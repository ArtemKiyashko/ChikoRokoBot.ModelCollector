using System;
using Azure;
using Azure.Data.Tables;

namespace ChikoRokoBot.ModelCollector.Models
{
    public class DropTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Finish { get; set; }
        public DateTime Start { get; set; }
        public int? Toyid { get; set; }
        public int? BlindBoxId { get; set; }
        public string Mechanic { get; set; }
        public string Slug { get; set; }
        public RarityTypes? RarityType { get; set; }
        public string DropJson { get; set; }
        public string ModelUrlUsdz { get; set; }
        public string ModelUrlGlb { get; set; }
        public string AlternativeCollect { get; set; }
    }
}

