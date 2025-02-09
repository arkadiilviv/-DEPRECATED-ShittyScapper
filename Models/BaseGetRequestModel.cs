using System.Text.Json.Serialization;

namespace ShittyApi.Models
{
    public class Param{
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public object Value { get; set; }
    }

    public class BaseGetRequestModel
    {
        [JsonPropertyName("params")]
        public Param[] Params { get; set; }
    }
}