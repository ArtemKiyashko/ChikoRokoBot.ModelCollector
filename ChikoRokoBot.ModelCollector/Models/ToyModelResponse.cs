using System.Text.Json.Serialization;

namespace ChikoRokoBot.ModelCollector.Models
{
    public record ToyModelResponse(
        [property: JsonPropertyName("toyId")] int ToyId,
        [property: JsonPropertyName("archiveName")] string ArchiveName,
        [property: JsonPropertyName("hash")] string Hash
    );
}

