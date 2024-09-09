using System.Threading.Tasks;
using Azure.Data.Tables;
using ChikoRokoBot.ModelCollector.Clients;
using ChikoRokoBot.ModelCollector.Models;
using ChikoRokoBot.ModelCollector.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChikoRokoBot.ModelCollector
{
    public class ModelCollector
    {
        private readonly TableClient _tableClient;
        private readonly ILogger<ModelCollector> _logger;
        private readonly ChikoRokoClient _chikoRokoClient;
        private readonly ModelCollectorOptions _options;

        public ModelCollector(
            TableClient tableClient,
            ILogger<ModelCollector> logger,
            IOptions<ModelCollectorOptions> options,
            ChikoRokoClient chikoRokoClient)
        {
            _tableClient = tableClient;
            _logger = logger;
            _chikoRokoClient = chikoRokoClient;
            _options = options.Value;
        }

        [FunctionName("ModelCollector")]
        public async Task Run([TimerTrigger("%FUNCTION_SCHEDULE%", RunOnStartup = false)]TimerInfo myTimer)
        {
            var allDrops = _tableClient.QueryAsync<DropTableEntity>(
                drop => drop.PartitionKey.Equals(_options.DefaultPartitionKey)
                && (drop.ModelUrlGlb == null || drop.ModelUrlUsdz == null)
                && drop.Mechanic != "BLINDBOX" );

            await foreach (var drop in allDrops)
            {
                if (!drop.Toyid.HasValue) continue;

                var modelUrlBase = await _chikoRokoClient.GetModelsBaseUrl(drop.Toyid.Value);

                if (string.IsNullOrEmpty(modelUrlBase)) continue;

                drop.ModelUrlGlb = $"{modelUrlBase}/scene.glb";
                drop.ModelUrlUsdz = $"{modelUrlBase}/scene.usdz";

                _logger.LogInformation($"Drop {drop.PartitionKey}-{drop.RowKey} updated. New value glb: {drop.ModelUrlGlb} New value usdz: {drop.ModelUrlUsdz} ");

                await _tableClient.UpdateEntityAsync(drop, drop.ETag);
            }
        }
    }
}

