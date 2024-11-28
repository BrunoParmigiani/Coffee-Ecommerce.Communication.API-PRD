using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class Row
    {
        [JsonPropertyName("elements")]
        public DistanceData[] Elements { get; set; }
    }
}
