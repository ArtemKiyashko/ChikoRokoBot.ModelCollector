using System.ComponentModel;

namespace ChikoRokoBot.ModelCollector.Models
{
    public enum RarityTypes
    {
        [Description("Common")]
        Common = 2,
        [Description("Rare")]
        Rare = 3,
        [Description("Super Rare")]
        SuperRare = 4,
        [Description("Epic")]
        Epic = 5,
        [Description("Legendary")]
        Legendary = 6,
        [Description("Mythical")]
        Mythical = 7,
        [Description("One And Only")]
        OneAndOnly = 8
    }
}

