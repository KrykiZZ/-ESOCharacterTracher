using System.Collections.Generic;

namespace CharacterTracker.Client.AddonDataParser.Models
{
    public class ESOAccount
    {
        public string Name { get; set; } = null!;
        public List<ESOCharacter> Characters { get; set; }

        public ESOAccount(string name)
        {
            Name = name;
            Characters = new();
        }
    }
}
