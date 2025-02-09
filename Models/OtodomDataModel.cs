using System.Text.Json.Serialization;

namespace ShittyApi.Models
{
    public class OtodomDataModel
    {
        [JsonPropertyName("title")]
        public string Name { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; }
        [JsonPropertyName("currency")]
        public string Currency { get; set; }
    }
}