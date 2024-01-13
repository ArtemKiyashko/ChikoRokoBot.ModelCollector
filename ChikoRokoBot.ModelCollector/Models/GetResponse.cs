using System.Text.Json.Serialization;

namespace ChikoRokoBot.ModelCollector.Models
{
    public class GetResponse<T> where T : class
    {
        [JsonPropertyName("result")]
        public Result<T> Result { get; set; }
    }

    public class Result<T> where T : class
    {
        [JsonPropertyName("data")]
        public Data<T> Data { get; set; }
    }

    public class Data<T> where T : class
    {
        [JsonPropertyName("json")]
        public T Json { get; set; }
    }
}

