using System;
using Newtonsoft.Json;

namespace MDAR_AcuityServiceAPI.Models
{
    public class Appointment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("appointmentTypeID")]
        public int AppointmentTypeId { get; set; }

        [JsonProperty("datetime")]
        public DateTime Datetime { get; set; }

        [JsonProperty("calendar")]
        public string Calendar { get; set; }

        [JsonProperty("calendarID")]
        public int CalendarId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        //"id": 54321,
        //"firstName": "Bob",
        //"lastName": "McTest",
        //"phone": "",
        //"email": "bob.mctest@example.com",
        //"date": "June 17, 2013",
        //"time": "10:15am",
        //"endTime": "11:15am",
        //"dateCreated": "July 2, 2013",
        //"datetime": "2013-06-17T10:15:00-0700",
        //"price": "10.00",
        //"paid": "no",
        //"amountPaid": "0.00",
        //"type": "Regular Visit",
        //"appointmentTypeID": 1,
        //"classID": null,
        //"duration": "60",
        //"calendar": "My Calendar",
        //"calendarID": 27238,
    }
}