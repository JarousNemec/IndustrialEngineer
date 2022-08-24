using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IndustrialEnginner;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.Items;

namespace GameEngine_druhypokus.Factories
{
    public class ItemFactory
    {
        public static ItemRegistry LoadItems(string path, GameData gameData)
        {
            ItemRegistry itemRegistry = new ItemRegistry();
            var presets = LoadJson(path);
            
            itemRegistry.Log = Log.Setup(ItemSetup(presets, "Log", gameData));
            itemRegistry.Registry.Add(itemRegistry.Log);
            
            return itemRegistry;
        }

        private static List<ItemPreset> LoadJson(string path)
        {
            List<ItemPreset> presets;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                presets = JsonSerializer.Deserialize<List<ItemPreset>>(json);
            }

            return presets;
        }

        private static Item ItemSetup(List<ItemPreset> presets, string name, GameData gameData)
        {
            var preset = presets.Find(x => x.name == name);
            return new Item(preset.id, preset.name, gameData.GetSprites()[name]);
        }
    }
    public class ItemPreset
    {
        public string name { get; set; }
        public int id { get; set; }
    }
}