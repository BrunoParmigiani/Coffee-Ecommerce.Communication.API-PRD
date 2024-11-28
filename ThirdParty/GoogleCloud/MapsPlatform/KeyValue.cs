using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class KeyValue
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}
