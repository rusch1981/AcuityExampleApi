using Newtonsoft.Json;

namespace MDAR_AcuityServiceAPI.Models
{
    public class Block
    {
        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("calendarID")]
        public string CalendarId { get; set; }
    }
}