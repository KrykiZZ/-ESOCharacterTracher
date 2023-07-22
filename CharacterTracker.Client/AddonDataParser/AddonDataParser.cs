using CharacterTracker.Client.AddonDataParser.Models;
using NLua;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace CharacterTracker.Client.AddonDataParser
{
    public class AddonDataParser
    {
        public static List<ESOAccount>? ParseAddonData()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Elder Scrolls Online", "live", "SavedVariables", "CharactersTracker.lua");

            if (!File.Exists(path))
                return null;

            Lua lua = new Lua();
            lua.DoFile(path);

            var accounts = new List<ESOAccount>();
            var table = lua.GetTable("CharactersTracker_Data");

            foreach (KeyValuePair<object, object> accountKVP in (LuaTable)table["Default"])
            {
                var account = new ESOAccount((string)accountKVP.Key);

                foreach (KeyValuePair<object, object> characterKVP in (LuaTable)accountKVP.Value)
                {
                    var character = new ESOCharacter(ulong.Parse((string)characterKVP.Key));
                    foreach (KeyValuePair<object, object> baseData in (LuaTable)characterKVP.Value)
                    {
                        if (((string)baseData.Key) == "$LastCharacterName")
                        {
                            character.Name = (string)baseData.Value;
                            continue;
                        }

                        character.WorldName = (string)baseData.Key;
                        foreach (KeyValuePair<object, object> savedData in (LuaTable)baseData.Value)
                        {
                            ParseMountData(savedData, ref character);
                            ParseCraftingData(savedData, ref character);
                            ParseDailyLFG(savedData, ref character);
                            ParseCurrencyData(savedData, ref character);
                        }
                    }

                    account.Characters.Add(character);
                }

                accounts.Add(account);
            }

            return accounts;
        }

        private static void ParseMountData(KeyValuePair<object, object> savedData, ref ESOCharacter character)
        {
            switch ((string)savedData.Key)
            {
                case "speedBonus":
                {
                    character.Mount.Speed = (long)savedData.Value;
                    break;
                }
                case "maxSpeedBonus":
                {
                    character.Mount.MaxSpeed = (long)savedData.Value;
                    break;
                }
                case "capacityBonus":
                {
                    character.Mount.Capacity = (long)savedData.Value;
                    break;
                }
                case "maxCapacityBonus":
                {
                    character.Mount.MaxCapacity = (long)savedData.Value;
                    break;
                }
                case "staminaBonus":
                {
                    character.Mount.Stamina = (long)savedData.Value;
                    break;
                }
                case "maxStaminaBonus":
                {
                    character.Mount.MaxStamina = (long)savedData.Value;
                    break;
                }
                case "lastMountTrain":
                {
                    character.Mount.LastTrained = DateTime.ParseExact((string)savedData.Value, "HH:mm:ss dd.MM.yyyy", CultureInfo.InvariantCulture);
                    break;
                }
            }
        }

        private static void ParseCraftingData(KeyValuePair<object, object> savedData, ref ESOCharacter character)
        {
            switch ((string)savedData.Key)
            {
                case "alchemyRank":
                {
                    character.Crafting.Alchemy = (long)savedData.Value;
                    break;
                }
                case "enchantingRank":
                {
                    character.Crafting.Enchanting = (long)savedData.Value;
                    break;
                }
                case "blacksmithingRank":
                {
                    character.Crafting.Blacksmithing = (long)savedData.Value;
                    break;
                }
                case "clothingRank":
                {
                    character.Crafting.Clothing = (long)savedData.Value;
                    break;
                }
                case "provisioningRank":
                {
                    character.Crafting.Provisioning = (long)savedData.Value;
                    break;
                }
                case "woodworkingRank":
                {
                    character.Crafting.Woodworking = (long)savedData.Value;
                    break;
                }
                case "jewelryRank":
                {
                    character.Crafting.Jewelry = (long)savedData.Value;
                    break;
                }
            }
        }

        private static void ParseDailyLFG(KeyValuePair<object, object> savedData, ref ESOCharacter character)
        {
            switch ((string)savedData.Key)
            {
                case "dailyDungeonRemainingTime":
                {
                    character.LFG.Dungeon = (long)savedData.Value;
                    break;
                }
                case "dailyBattlebroundRemainingTime":
                {
                    character.LFG.Battleground = (long)savedData.Value;
                    break;
                }
            }
        }

        private static void ParseCurrencyData(KeyValuePair<object, object> savedData, ref ESOCharacter character)
        {
            switch ((string)savedData.Key)
            {
                case "gold":
                {
                    character.Currency.Gold = (long)savedData.Value;
                    break;
                }
                case "alliancePoints":
                {
                    character.Currency.AlliancePoints = (long)savedData.Value;
                    break;
                }
                case "telvar":
                {
                    character.Currency.TelVar = (long)savedData.Value;
                    break;
                }
                case "vouchers":
                {
                    character.Currency.Vouchers = (long)savedData.Value;
                    break;
                }
            }
        }
    }
}
