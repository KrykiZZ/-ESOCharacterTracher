using System;

namespace CharacterTracker.Client.AddonDataParser.Models
{
    public class ESOMountData
    {
        public long MaxSpeed { get; set; }
        public long Speed { get; set; }

        public long MaxCapacity { get; set; }
        public long Capacity { get; set; }

        public long MaxStamina { get; set; }
        public long Stamina { get; set; }

        public DateTime LastTrained { get; set; }
    }
}
