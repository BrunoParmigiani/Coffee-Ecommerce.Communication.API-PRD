using System.Text.Json.Serialization;

namespace Coffee_Ecommerce.Communication.API.ThirdParty.GoogleCloud.MapsPlatform
{
    public sealed class DistanceMatrixResponse
    {
        [JsonPropertyName("destination_addresses")]
        public string[] DestinationAddresses { get; set; }

        [JsonPropertyName("origin_addresses")]
        public string[] OriginAddresses { get; set; }

        [JsonPropertyName("rows")]
        public Row[] Rows { get; set; }

        public int GetDistance()
        {
            return Rows[0].Elements[0].Distance.Value;
        }
    }
}
