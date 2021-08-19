using Newtonsoft.Json;

namespace FoodService
{
    public class FoodTruck
    {
        [JsonProperty("objectid", Required = Required.Default)]
        public int TruckId { get; set; }

        [JsonProperty("block", Required = Required.Default)]
        public int Block { get; set; }

        [JsonProperty("applicant", Required = Required.Default)]
        public string TruckName { get; set; }

        [JsonProperty("locationdescription", NullValueHandling=NullValueHandling.Ignore)]
        public string Address { get; set; }
    }
}
