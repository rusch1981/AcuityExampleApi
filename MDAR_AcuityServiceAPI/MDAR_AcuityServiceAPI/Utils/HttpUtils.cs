using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using MDAR_AcuityServiceAPI.Classes;
using MDAR_AcuityServiceAPI.Models;
using Newtonsoft.Json;

namespace MDAR_AcuityServiceAPI.Utils
{
    public class HttpUtils
    {
        internal static Appointment DeserializeAppointment(string query, Logging logger)
        {
            logger.LogMessage("Deserlizing Acuity Appointment");
            var decodedQuery = HttpUtility.UrlDecode(query);
            var appointment = JsonConvert.DeserializeObject<Appointment>(decodedQuery);
            logger.Log(appointment);
            return appointment;
        }

        internal static IEnumerable<Appointment> DeserializeAppointments(string query, Logging logger)
        {
            logger.LogMessage("Deserlizing Acuity Appointments");
            var decodedQuery = HttpUtility.UrlDecode(query);
            var appointments =
                JsonConvert.DeserializeObject<IEnumerable<Appointment>>(decodedQuery) as List<Appointment>;
            if (appointments != null && appointments.Any())
            {
                logger.Log(appointments);
                return appointments;
            }
            logger.LogMessage("No appointments identified");
            return new List<Appointment>();
        }

        internal static IEnumerable<Calendar> DeserializeCalendars(string query, Logging logger)
        {
            logger.LogMessage("Deserlizing Acuity Calendars");
            var decodedQuery = HttpUtility.UrlDecode(query);
            var calendars = JsonConvert.DeserializeObject<IEnumerable<Calendar>>(decodedQuery) as List<Calendar>;
            if (calendars != null && calendars.Any())
            {
                logger.Log(calendars);
                return calendars;
            }
            logger.LogMessage("No Calendars identified");
            return new List<Calendar>();
        }


        public static IEnumerable<Availability> DeserializeAvailabilities(string query, Logging logger)
        {
            logger.LogMessage("Deserlizing Acuity Availabilities");
            var decodedQuery = HttpUtility.UrlDecode(query);
            var availabilities =
                JsonConvert.DeserializeObject<IEnumerable<Availability>>(decodedQuery) as List<Availability>;
            if (availabilities != null && availabilities.Any())
            {
                logger.Log(availabilities);
                return availabilities;
            }
            logger.LogMessage("No Availabilites Identified");
            return new List<Availability>();
        }

        public static string GetBasicCredentials()
        {
            var userName = ConfigurationManager.AppSettings["UserName"];
            var password = ConfigurationManager.AppSettings["Password"];
            var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                .GetBytes($"{userName}:{password}"));

            return $"Basic {encoded}";
        }
    }
}