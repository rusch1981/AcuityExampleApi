using Newtonsoft.Json;

namespace MDAR_AcuityServiceAPI.Models
{
    public class Calendar
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("repolyTo")]
        public string ReplyTo { get; set; }


        //[
        //{
        //    "id": 1234,
        //    "name": "Emily",
        //    "email": "",
        //    "replyTo": ""
        //},
        //{
        //    "id": 4321,
        //    "name": "Joe",
        //    "email": "joecontractor@example.com",
        //    "replyTo": "joecontractor@example.com"
        //}
        //]
    }
}