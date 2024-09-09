using System;
namespace ChikoRokoBot.ModelCollector.Options
{
	public class ModelCollectorOptions
	{
        public string DropsTableName { get; set; } = "drops";
        public Uri ChikoRokoBaseAddress { get; set; } = new Uri("https://r.toys/");
        public string ChikoRokoSessionId { get; set; }
        public string TableServiceConnection { get; set; } = "UseDevelopmentStorage=true";
        public string DefaultPartitionKey { get; set; } = "primary";
    }
}

