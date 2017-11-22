using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using MDAR_AcuityServiceAPI.Models;

namespace MDAR_AcuityServiceAPI.Classes
{
    public class HookManager
    {
        private ApiManager _apiManager = new ApiManager();
        private Logging _logger;

        public void Setlogger(Logging logger)
        {
            _logger = logger;
            _apiManager._logger = logger;
        }

        public bool IsvalidRequest { get; private set; }

        public HttpStatusCode StatusCode { get; private set; } = HttpStatusCode.OK;
        public string Message { get; internal set; } = "";

        public void ProcessTasks(Notification notification)
        {
            if (notification.Action.Equals(NotificationConstants.Scheduled, StringComparison.CurrentCultureIgnoreCase))
            {
                MaximumDailyAppointmentsRoutine(notification);
            }
            else if (notification.Action.Equals(NotificationConstants.Changed,
                StringComparison.CurrentCultureIgnoreCase))
            {
            }
            else if (notification.Action.Equals(NotificationConstants.Canceled,
                StringComparison.CurrentCultureIgnoreCase))
            {
            }
            else if (notification.Action.Equals(NotificationConstants.Rescheduled,
                StringComparison.CurrentCultureIgnoreCase))
            {
            }
        }

        private void MaximumDailyAppointmentsRoutine(Notification notification)
        {
            _logger.LogMessage(
                $"Beginning Maximum Daily Appointments Routine (Max: {ConfigurationManager.AppSettings["MaxDailyAppointments"]})...");
            var appointment = _apiManager.GetAppointment(notification.Id);

            var calendars = new List<Calendar>();
            var calendarNames = ConfigurationManager.AppSettings["Calendarnames"].Split('.').ToList();
            foreach (var calendar in _apiManager.GetCalendars())
            {
                if (calendarNames.Contains(calendar.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    calendars.Add(calendar);
                }
            }

            var appointments = _apiManager.GetAppointments(calendars, appointment.Datetime).ToList();
            if (appointments.Count >= int.Parse(ConfigurationManager.AppSettings["MaxDailyAppointments"]))
            {
                _logger.LogMessage(
                    $"Maximum Daily Appointments met for {appointment.Datetime}. (Appt.Count: {appointments.Count})");

                var appointmentTypeIds = ConfigurationManager.AppSettings["AppointmentTypeIds"].Split(',');
                var allowedbookingPerAppointment =
                    int.Parse(ConfigurationManager.AppSettings["AllowedbookingPerAppointment"]);
                for (var i = 1; i < allowedbookingPerAppointment; ++i)
                {
                    foreach (var appointmentTypeId in appointmentTypeIds)
                    {
                        foreach (var calendar in calendars)
                        {
                            var availabilities = _apiManager
                                .GetAvailabilities(calendar, appointment.Datetime, appointmentTypeId).ToList();
                            if (availabilities.Any())
                                _apiManager.CreateAppointmentsForAvailabilities(calendar, availabilities,
                                    appointmentTypeId);
                        }
                    }
                }
                _logger.LogMessage("MaximumDailyAppointmentsRoutine completed successfully.");
            }
            else
            {
                _logger.LogMessage($"Maximum Daily Appointments not met. (Appt.Count: {appointments.Count})");
            }
            StatusCode = HttpStatusCode.OK;
            Message = "OK";
        }

        public void ValidateRequest(HttpContext httpContext, string requestBody)
        {
            if (requestBody == null)
            {
                _logger.LogMessage("Post containing a NULL body recieved");
                IsvalidRequest = false;
                StatusCode = HttpStatusCode.BadRequest;
                Message = "Request was null";
                return;
            }

            _logger.LogMessage($"Notication recieved: {requestBody}");
            _logger.LogMessage("Authenticating...");

            var encoding = new ASCIIEncoding();
            var key = encoding.GetBytes(ConfigurationManager.AppSettings["Password"]);
            var message = encoding.GetBytes(requestBody);
            string token;
            using (var hmacsha256 = new HMACSHA256(key))
            {
                var hashmessage = hmacsha256.ComputeHash(message);
                token = Convert.ToBase64String(hashmessage);
            }

            var authHeader = httpContext.Request.Headers["X-Acuity-Signature"];
            if (string.IsNullOrWhiteSpace(authHeader) || !token.Equals(authHeader))
            {
                var authenticationMessage = $"Authentication Failed: {Environment.NewLine}" +
                                            $"The AuthHeader does not match the current token.{Environment.NewLine}" +
                                            $"AuthHeader: {Environment.NewLine}{authHeader}{Environment.NewLine}" +
                                            $"MyToken: {Environment.NewLine}{token}{Environment.NewLine}" +
                                            $"UserHostAddress:  {httpContext.Request.UserHostAddress}{Environment.NewLine}" +
                                            $"UserHostName: {httpContext.Request.UserHostName}{Environment.NewLine}";
                _logger.LogFatalError(new Exception(authenticationMessage));
                IsvalidRequest = false;
                StatusCode = HttpStatusCode.Unauthorized;
                Message = "Unauthorized";
                return;
            }
            _logger.LogMessage("Authentication Successful!");
            StatusCode = HttpStatusCode.OK;
            Message = "OK";
            IsvalidRequest = true;
        }

        public void Terminate()
        {
            _logger.LogMessage(
                $"Sending HttpResponse and Terminating Process.{Environment.NewLine}{Environment.NewLine}");
        }
    }
}