using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using IndustrialEngineer.Blocks;
using IndustrialEnginner;
using IndustrialEnginner.Enums;
using IndustrialEnginner.GameEntities;
using SFML.Graphics;

namespace IndustrialEngineer.Factories
{
    public class EntityFactory
    {
        public static PlaceableEntityRegistry LoadEntities(string path, GameData data)
        {
            var presets = LoadJson(path);
            var properties = MakePropertiesList(data, presets);
            var registry = new PlaceableEntityRegistry();

            registry.Drill = new Drill(properties.Find(x => x.Name == "Drill"));
            registry.Registry.Add(registry.Drill);

            registry.Furnace = new Furnace(properties.Find(x => x.Name == "Furnace"));
            registry.Registry.Add(registry.Furnace);
            
            registry.WoodenPlatform = new WoodenPlatform(properties.Find(x => x.Name == "WoodenPlatform"));
            registry.Registry.Add(registry.WoodenPlatform);
            return registry;
        }

        private static List<PlaceableEntityProperties> MakePropertiesList(GameData data, List<PlaceableEntityPreset> presets)
        {
            List<PlaceableEntityProperties> propertiesList = new List<PlaceableEntityProperties>();
            foreach (var preset in presets)
            {
                List<Sprite> states = new List<Sprite>();
                foreach (var state in preset.States)
                {
                    states.Add(data.GetSprites()[state]);
                }
                propertiesList.Add(new PlaceableEntityProperties(data.GetSprites()[preset.Texture],states.ToArray(),preset.Name, preset.Id, (BlockType)preset.CanBePlacedOnType, preset.DropItemId, preset.CanStepOn));
            }
            return propertiesList;
        }

        private static List<PlaceableEntityPreset> LoadJson(string path)
        {
            List<PlaceableEntityPreset> presetsList;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                presetsList = JsonSerializer.Deserialize<List<PlaceableEntityPreset>>(json);
            }

            return presetsList;
        }

        private class PlaceableEntityPreset
        {
            public string Texture { get; set; }
            public string[] States { get; set; }
            public short Id { get; set; }
            public string Name { get; set; }
            public int CanBePlacedOnType { get; set; }
            public int DropItemId { get; set; }
            public bool CanStepOn { get; set; }
        }
    }
}