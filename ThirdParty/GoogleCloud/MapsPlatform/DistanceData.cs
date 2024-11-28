using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class DistanceData
    {
        [JsonPropertyName("distance")]
        public KeyValue Distance { get; set; }

        [JsonPropertyName("duration")]
        public KeyValue Duration { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
