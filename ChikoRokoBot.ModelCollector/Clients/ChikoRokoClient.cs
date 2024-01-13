using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ChikoRokoBot.ModelCollector.Models;
using System.Text.Json;

namespace ChikoRokoBot.ModelCollector.Clients
{
    public class ChikoRokoClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChikoRokoClient> _logger;

        public ChikoRokoClient(HttpClient httpClient, ILogger<ChikoRokoClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetModelsBaseUrl(int toyId)
        {
            var request = new RequestValueToy(new RequestParameterToyId(toyId));
            var dictionaryRequest = new Dictionary<int, RequestValueToy>
            {
                { 0, request }
            };
            var json = JsonSerializer.Serialize(dictionaryRequest);

            try
            {
                var response = await _httpClient.GetStringAsync($"api/v2/ToyManager.getModel?batch=1&input={json}");
                var responseObject = JsonSerializer.Deserialize<List<GetResponse<ToyModelResponse>>>(response);

                if (string.IsNullOrEmpty(responseObject[0].Result.Data.Json.Hash))
                    return $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}";
                else
                    return $"https://chikoroko.b-cdn.net/toys/models/{responseObject[0].Result.Data.Json.ArchiveName}/{responseObject[0].Result.Data.Json.Hash}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot get toy model link", ex);
                return default;
            }
        }
    }
}

