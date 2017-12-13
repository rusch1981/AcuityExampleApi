using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using MDAR_AcuityServiceAPI.Models;
using MDAR_AcuityServiceAPI.Utils;

namespace MDAR_AcuityServiceAPI.Classes
{
    public class Logging
    {
        public StringBuilder _serviceLog = new StringBuilder();

        //    string filePath = @"C:\test.csv";
        //2               string delimiter = ",";
        //3
        //4               string[][] output = new string[][]{
        //    5                   new string[]{"Col 1 Row 1", "Col 2 Row 1", "Col 3 Row 1"},
        //    6                   new string[]{"Col1 Row 2", "Col2 Row 2", "Col3 Row 2"}
        //    7               };
        //8               int length = output.GetLength(0);
        //9               StringBuilder sb = new StringBuilder();
        //10              for (int index = 0; index < length; index++)
        //    11                  sb.AppendLine(string.Join(delimiter, output[index]));
        //12
        //13              File.WriteAllText(filePath, sb.ToString());

        public void LogMessage(string message)
        {
            _serviceLog.Append(DateTime.Now + " " + message + Environment.NewLine);
        }

        public void Log(Notification notification)
        {

            _serviceLog.Append($"{DateTime.Now} Notification: {Environment.NewLine}" +
                          $"{notification.Action}{Environment.NewLine}" +
                          $"{notification.AppointmentTypeId}{Environment.NewLine}" +
                          $"{notification.CalendarId}{Environment.NewLine}" +
                          $"{notification.Id}{Environment.NewLine}{Environment.NewLine}");
        }

        internal void Log(IEnumerable<Calendar> calendars)
        {
            var logCalendars = calendars as IList<Calendar> ?? calendars.ToList();
            if (logCalendars.Any())
            {
                _serviceLog.Append($"{DateTime.Now} Calendars: {Environment.NewLine}");
                foreach (var calendar in logCalendars)
                {
                    _serviceLog.Append($"{calendar.Name}{Environment.NewLine}");
                }
            }
            else
            {
                _serviceLog.Append("No Calendars were located");
            }
            _serviceLog.Append(Environment.NewLine);
        }

        internal void Log(IEnumerable<Appointment> appointments)
        {
            var logAppointments = appointments as IList<Appointment> ?? appointments.ToList();
            if (logAppointments.Any())
            {
                _serviceLog.Append($"Appointments: {Environment.NewLine}");
                foreach (var appointment in logAppointments)
                {
                    _serviceLog.Append($"{appointment.Datetime}{Environment.NewLine}" +
                                   $"Calendar: {appointment.Calendar}{Environment.NewLine}" +
                                   $"AppointmentId: {appointment.Id}{Environment.NewLine}" +
                                   $"Name: {appointment.LastName},{appointment.FirstName}{Environment.NewLine}" +
                                   $"AppointmentType: {appointment.AppointmentTypeId}{Environment.NewLine}{Environment.NewLine}");
                }
            }
            else
            {
                _serviceLog.Append("No Appointments were located");
            }
        }

        public void Log(Appointment appointment)
        {
            _serviceLog.Append($"{DateTime.Now} Appointment: {Environment.NewLine}" +
                          $"{appointment.Datetime}{Environment.NewLine}" +
                          $"{appointment.CalendarId}{Environment.NewLine}" +
                          $"{appointment.Id}{Environment.NewLine}" +
                          $"{appointment.LastName},{appointment.FirstName}{Environment.NewLine}" +
                          $"{appointment.AppointmentTypeId}{Environment.NewLine}{Environment.NewLine}");
        }

        public void Log(List<Availability> availabilities)
        {;
            if (availabilities.Any())
            {
                _serviceLog.Append($"Availabilities: {Environment.NewLine}");
                foreach (var availability in availabilities)
                {
                    _serviceLog.Append($"{DateTime.Now} Availability: {availability.Time}{Environment.NewLine}");
                }
            }
            else
            {
                _serviceLog.Append("No Availabilities were located");
            }
            _serviceLog.Append(Environment.NewLine);
        }

        public void LogLog(string responseMessage)
        {
            EmailUtil.SendLogEmail(responseMessage + Environment.NewLine + _serviceLog.ToString());
        }

        public void LogFatalError(Exception exception)
        {
            LogMessage(Environment.NewLine
                       + "**********************************************************************************"
                       + Environment.NewLine
                       + "Intervnal Server Error");
            LogMessage(exception.ToString());

            EmailUtil.SendExceptionEmail(exception, _serviceLog.ToString());
        }
    }
}