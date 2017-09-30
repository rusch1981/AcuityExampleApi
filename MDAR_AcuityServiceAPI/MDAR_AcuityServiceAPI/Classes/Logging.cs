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
        private readonly string _serviceHistoryPath = ConfigurationManager.AppSettings["LogDirectory"];

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

        public void LogMessage(string message, int newLineCount = 1)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var logMessage = DateTime.Now + " " + message;

            if (newLineCount < 1)
            {
                File.AppendAllText(filePath, logMessage);
            }
            else
            {
                for (var i = 0; i < newLineCount; i++)
                {
                    logMessage = logMessage + Environment.NewLine;
                }
                File.AppendAllText(filePath, logMessage);
            }
        }

        public void Log(Notification notification)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var saveTxt = $"{DateTime.Now} Notification: {Environment.NewLine}" +
                          $"{notification.Action}{Environment.NewLine}" +
                          $"{notification.AppointmentTypeId}{Environment.NewLine}" +
                          $"{notification.CalendarId}{Environment.NewLine}" +
                          $"{notification.Id}{Environment.NewLine}{Environment.NewLine}";

            File.AppendAllText(filePath, saveTxt);
        }

        internal void Log(IEnumerable<Calendar> calendars)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var saveTxt = new StringBuilder();
            var logCalendars = calendars as IList<Calendar> ?? calendars.ToList();
            if (logCalendars.Any())
            {
                saveTxt.Append($"{DateTime.Now} Calendars: {Environment.NewLine}");
                foreach (var calendar in logCalendars)
                {
                    saveTxt.Append($"{calendar.Name}{Environment.NewLine}");
                }
            }
            else
            {
                saveTxt.Append("No Calendars were located");
            }
            saveTxt.Append(Environment.NewLine);
            File.AppendAllText(filePath, saveTxt.ToString());
        }

        internal void Log(IEnumerable<Appointment> appointments)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var saveTxt = new StringBuilder();
            var logAppointments = appointments as IList<Appointment> ?? appointments.ToList();
            if (logAppointments.Any())
            {
                saveTxt.Append($"Appointments: {Environment.NewLine}");
                foreach (var appointment in logAppointments)
                {
                    saveTxt.Append($"{appointment.Datetime}{Environment.NewLine}" +
                                   $"Calendar: {appointment.Calendar}{Environment.NewLine}" +
                                   $"AppointmentId: {appointment.Id}{Environment.NewLine}" +
                                   $"Name: {appointment.LastName},{appointment.FirstName}{Environment.NewLine}" +
                                   $"AppointmentType: {appointment.AppointmentTypeId}{Environment.NewLine}{Environment.NewLine}");
                }
            }
            else
            {
                saveTxt.Append("No Appointments were located");
            }

            File.AppendAllText(filePath, saveTxt.ToString());
        }

        public void Log(Appointment appointment)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var saveTxt = $"{DateTime.Now} Appointment: {Environment.NewLine}" +
                          $"{appointment.Datetime}{Environment.NewLine}" +
                          $"{appointment.CalendarId}{Environment.NewLine}" +
                          $"{appointment.Id}{Environment.NewLine}" +
                          $"{appointment.LastName},{appointment.FirstName}{Environment.NewLine}" +
                          $"{appointment.AppointmentTypeId}{Environment.NewLine}{Environment.NewLine}";

            File.AppendAllText(filePath, saveTxt);
        }

        public void Log(List<Availability> availabilities)
        {
            var directory = new DirectoryInfo(_serviceHistoryPath);
            directory.Create();

            var filePath = $"{_serviceHistoryPath}{DateTime.Today:yyyyMMdd}.txt";

            if (!File.Exists(filePath)) File.Create(filePath).Close();

            var saveTxt = new StringBuilder();
            if (availabilities.Any())
            {
                saveTxt.Append($"Availabilities: {Environment.NewLine}");
                foreach (var availability in availabilities)
                {
                    saveTxt.Append($"{DateTime.Now} Availability: {availability.Time}{Environment.NewLine}");
                }
            }
            else
            {
                saveTxt.Append("No Availabilities were located");
            }
            saveTxt.Append(Environment.NewLine);
            File.AppendAllText(filePath, saveTxt.ToString());
        }

        public void LogFatalError(Exception exception)
        {
            LogMessage(Environment.NewLine
                       + "**********************************************************************************"
                       + Environment.NewLine
                       + "Intervnal Server Error");
            LogMessage(exception.ToString());

            EmailUtil.SendEmail(exception);
        }
    }
}