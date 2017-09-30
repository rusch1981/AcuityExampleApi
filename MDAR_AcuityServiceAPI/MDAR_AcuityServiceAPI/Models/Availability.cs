using Newtonsoft.Json;

namespace MDAR_AcuityServiceAPI.Models
{
    public class Availability
    {
        [JsonProperty("time")]
        public string Time { get; set; }
    }
}