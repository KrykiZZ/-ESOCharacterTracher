namespace CharacterTracker.Client.AddonDataParser.Models
{
    public class ESOCharacter
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = null!;
        public string WorldName { get; set; } = null!;

        public ESOCraftData Crafting { get; set; }
        public ESOMountData Mount { get; set; }
        public ESOCurrencyData Currency { get; set; }
        public ESODailyLFGData LFG { get; set; }

        public ESOCharacter(ulong id)
        {
            Id = id;

            Mount = new();
            Crafting = new();
            LFG = new();
            Currency = new();
        }
    }
}
