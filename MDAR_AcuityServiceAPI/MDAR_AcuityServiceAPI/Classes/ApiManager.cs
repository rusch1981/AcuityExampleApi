using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using MDAR_AcuityServiceAPI.Models;
using MDAR_AcuityServiceAPI.Utils;

namespace MDAR_AcuityServiceAPI.Classes
{
    public class ApiManager
    {
        public Logging _logger;

        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }

        public IEnumerable<Appointment> GetAppointments(List<Calendar> calendars, DateTime date)
        {
            var appointments = new List<Appointment>();
            _logger.LogMessage("Attempting retrieve Appointments...");
            foreach (var calendar in calendars)
            {
                var request =
                    (HttpWebRequest) WebRequest.Create(ConfigurationManager.AppSettings["AppointmentsApi"] +
                                                       $"?minDate={date:yyyy-MM-dd}&maxDate={date:yyyy-MM-dd}&calendarID={calendar.Id}");
                request.Headers.Add("Authorization", HttpUtils.GetBasicCredentials());

                try
                {
                    using (var response = request.GetResponse() as HttpWebResponse)
                    {
                        _logger.LogMessage($"Request HttpCode: {response?.StatusCode}");
                        var responseStream = response?.GetResponseStream();
                        if (responseStream == null)
                        {
                            return appointments;
                        }
                        using (var reader = new StreamReader(responseStream))
                        {
                            var receiveContent = reader.ReadToEnd();
                            appointments.AddRange(HttpUtils.DeserializeAppointments(receiveContent, _logger));
                        }
                    }
                }
                catch (WebException e)
                {
                    _logger.LogMessage(e.Message);
                    var errorResponse = e.Response;
                    using (var stream = errorResponse.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            var reader = new StreamReader(stream);
                            var errorText = reader.ReadToEnd();
                            _logger.LogMessage(errorText);
                        }
                    }
                }
            }
            return appointments;
        }

        public IEnumerable<Calendar> GetCalendars()
        {
            IEnumerable<Calendar> calendars = new List<Calendar>();
            _logger.LogMessage("Attempting retrieve Calendars...");
            var request =
                (HttpWebRequest) WebRequest.Create(ConfigurationManager.AppSettings["CalendarsApi"]);
            request.Headers.Add("Authorization", HttpUtils.GetBasicCredentials());
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    _logger.LogMessage($"Request HttpCode: {response?.StatusCode}");

                    var responseStream = response?.GetResponseStream();
                    if (responseStream == null)
                    {
                        return calendars;
                    }
                    using (var reader = new StreamReader(responseStream))
                    {
                        var content = reader.ReadToEnd();
                        calendars = HttpUtils.DeserializeCalendars(content, _logger);
                    }
                }
            }
            catch (WebException e)
            {
                _logger.LogMessage(e.Message);
                var errorResponse = e.Response;
                using (var stream = errorResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream);
                        var errorText = reader.ReadToEnd();
                        _logger.LogMessage(errorText);
                    }
                }
            }
            return calendars;
        }

        public Appointment GetAppointment(string id)
        {
            var appointment = new Appointment();
            _logger.LogMessage($"Attempting retrieve Appointment {id}...");

            var request =
                (HttpWebRequest) WebRequest.Create($"{ConfigurationManager.AppSettings["AppointmentsApi"]}/{id}");
            request.Headers.Add("Authorization", HttpUtils.GetBasicCredentials());
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    _logger.LogMessage($"Appointment Request HttpCode: {response?.StatusCode}");
                    var responseStream = response?.GetResponseStream();
                    if (responseStream == null)
                    {
                        return appointment;
                    }
                    using (var reader = new StreamReader(responseStream))
                    {
                        var receiveContent = reader.ReadToEnd();
                        appointment = HttpUtils.DeserializeAppointment(receiveContent, _logger);
                    }
                }
            }
            catch (WebException e)
            {
                _logger.LogMessage(e.Message);
                var errorResponse = e.Response;
                using (var stream = errorResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream);
                        var errorText = reader.ReadToEnd();
                        _logger.LogMessage(errorText);
                    }
                }
            }
            return appointment;
        }

        public IEnumerable<Availability> GetAvailabilities(Calendar calendar, DateTime date, string appointmentTypeId)
        {
            var availabilities = new List<Availability>();
            _logger.LogMessage(
                $"Attempting retrieve Availabilities for {calendar.Name} calendar on Date({date})...");

            var request =
                (HttpWebRequest) WebRequest.Create(ConfigurationManager.AppSettings["AvailabilityTimesApi"] +
                                                   $"?appointmentTypeID={appointmentTypeId}&calendarID={calendar.Id}&date={date:yyyy-MM-dd}");
            request.Headers.Add("Authorization", HttpUtils.GetBasicCredentials());
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    _logger.LogMessage($"Request HttpCode: {response?.StatusCode}");
                    var responseStream = response?.GetResponseStream();
                    if (responseStream == null)
                    {
                        return availabilities;
                    }
                    using (var reader = new StreamReader(responseStream))
                    {
                        var receiveContent = reader.ReadToEnd();
                        availabilities.AddRange(
                            HttpUtils.DeserializeAvailabilities(receiveContent, _logger));
                    }
                }
            }
            catch (WebException e)
            {
                _logger.LogMessage(e.Message);
                var errorResponse = e.Response;
                using (var stream = errorResponse.GetResponseStream())
                {
                    if (stream != null)
                    {
                        var reader = new StreamReader(stream);
                        var errorText = reader.ReadToEnd();
                        _logger.LogMessage(errorText);
                    }
                }
            }

            return availabilities;
        }

        //current contract
        //{
        //    "datetime": "2017-09-12T09:00:00-0400",
        //    "appointmentTypeID": 3359958,
        //    "firstName": "Adam",
        //    "lastName": "McTest",
        //    "email": "rusch1981@hotmail.com",
        //    "phone": "5555555555",
        //    "fields": [
        //    {"id": "3066534", "value": "NotApplicable"},
        //    {"id": "3376080", "value": "$0"},
        //    {"id": "3376089", "value": "Friend/Family"}
        //    ]
        //}
        public void CreateAppointmentsForAvailabilities(Calendar calendar, IEnumerable<Availability> availabilities,
            string appointmentTypeId)
        {
            _logger.LogMessage(
                $"Attempting create appointments for availabilities for Calendar : ({calendar.Name})...");
            foreach (var availability in availabilities)
            {
                var request =
                    (HttpWebRequest) WebRequest.Create(ConfigurationManager.AppSettings["CreateAppointmentApi"]);
                request.Headers.Add("Authorization", HttpUtils.GetBasicCredentials());
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    var json = "{\"datetime\": \"" + availability.Time + "\"," +
                               "\"appointmentTypeID\": " + appointmentTypeId + "," +
                               "\"firstName\": \"Adam\"," +
                               "\"lastName\": \"McTest\"," +
                               "\"email\": \"mdaracuity@hotmail.com\"," +
                               "\"phone\": \"5555555555\"";

                    var requirmentIds = ConfigurationManager.AppSettings["RequiredAppointmentElementIds"]
                        .Split(',');
                    var requirmentValues = ConfigurationManager.AppSettings["RequiredAppointmentElementValues"]
                        .Split(',');
                    if (requirmentIds.Length != requirmentValues.Length)
                    {
                        _logger.LogMessage("Error!  Throwing exception");
                        throw new Exception(
                            $"Configuration Exception.  RequiredAppointmentElementIds: {requirmentIds} " +
                            $"RequiredAppointmentElementValues: {requirmentValues} do not have the matching length Value");
                    }
                    if (requirmentIds.Any())
                    {
                        json += ",\"fields\": [";
                        for (var i = 0; i < requirmentIds.Length; i++)
                        {
                            json += "{\"id\": \"" + requirmentIds[i] + "\"," +
                                    "\"value\": \"" + requirmentValues[i] + "\"}";
                            json += (i + 1 == requirmentIds.Length ? "]" : ",");
                        }
                    }
                    json += "}";
                    _logger.LogMessage(json);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    try
                    {
                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            _logger.LogMessage($"Request HttpCode: {response?.StatusCode}");
                        }
                    }
                    catch (WebException e)
                    {
                        _logger.LogMessage(e.Message);
                        var errorResponse = e.Response;
                        using (var stream = errorResponse.GetResponseStream())
                        {
                            if (stream != null)
                            {
                                var reader = new StreamReader(stream);
                                var errorText = reader.ReadToEnd();
                                _logger.LogMessage(errorText);
                            }
                        }
                    }
                }
            }
        }
    }
}