using System.Text.Json.Serialization;

namespace ChikoRokoBot.ModelCollector.Models
{
    public record RequestValueToy(
        [property: JsonPropertyName("json")] RequestParameterToyId Parameter
    );
}

