using System.Text.Json.Serialization;

namespace Dashboard.Application.Slice.ExternalData
{
     public class PolygonResponse
    {
        [JsonPropertyName("results")]
        public List<PolygonAgg> Results { get; set; }
    }

    public class PolygonAgg
    {
        [JsonPropertyName("v")]
        public double Volume { get; set; }

        [JsonPropertyName("vw")]
        public double VolumeWeightedAveragePrice { get; set; }

        [JsonPropertyName("o")]
        public double Open { get; set; }

        [JsonPropertyName("c")]
        public double Close { get; set; }

        [JsonPropertyName("h")]
        public double High { get; set; }

        [JsonPropertyName("l")]
        public double Low { get; set; }

        [JsonPropertyName("t")]
        public long Timestamp { get; set; }

        [JsonPropertyName("n")]
        public int Transactions { get; set; }
    }
}
