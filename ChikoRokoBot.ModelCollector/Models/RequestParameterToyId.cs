using System.Text.Json.Serialization;

namespace ChikoRokoBot.ModelCollector.Models
{
    public record RequestParameterToyId(
        [property: JsonPropertyName("toyId")] int ToyId
    );
}

