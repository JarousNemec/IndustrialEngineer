using System.Collections.Generic;
using System.IO;
using SFML.Audio;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IndustrialEnginner.Blocks
{
    public class BlockFactory
    {
        public static BlockRegistry LoadBlocks(string path)
        {
            BlockRegistry blockRegistry = new BlockRegistry();
            var presets = LoadJson(path);

            blockRegistry.Bridge = Bridge.Setup(BlockSetup(presets, "Bridge"));
            blockRegistry.Registry.Add(blockRegistry.Bridge);

            blockRegistry.Coal = Coal.Setup(BlockSetup(presets, "Coal"));
            blockRegistry.Registry.Add(blockRegistry.Coal);

            blockRegistry.Copper = Copper.Setup(BlockSetup(presets, "Copper"));
            blockRegistry.Registry.Add(blockRegistry.Copper);

            blockRegistry.Diamond = Diamond.Setup(BlockSetup(presets, "Diamond"));
            blockRegistry.Registry.Add(blockRegistry.Diamond);

            blockRegistry.Emerald = Emerald.Setup(BlockSetup(presets, "Emerald"));
            blockRegistry.Registry.Add(blockRegistry.Emerald);

            blockRegistry.Gold = Gold.Setup(BlockSetup(presets, "Gold"));
            blockRegistry.Registry.Add(blockRegistry.Gold);

            blockRegistry.Grass = Grass.Setup(BlockSetup(presets, "Grass"));
            blockRegistry.Registry.Add(blockRegistry.Grass);

            blockRegistry.Sand = Sand.Setup(BlockSetup(presets, "Sand"));
            blockRegistry.Registry.Add(blockRegistry.Sand);

            blockRegistry.Stone = Stone.Setup(BlockSetup(presets, "Stone"));
            blockRegistry.Registry.Add(blockRegistry.Stone);

            blockRegistry.Tree = Tree.Setup(BlockSetup(presets, "Tree"));
            blockRegistry.Registry.Add(blockRegistry.Tree);

            blockRegistry.Water = Water.Setup(BlockSetup(presets, "Water"));
            blockRegistry.Registry.Add(blockRegistry.Water);

            blockRegistry.Workbench = Workbench.Setup(BlockSetup(presets, "Workbench"));
            blockRegistry.Registry.Add(blockRegistry.Workbench);

            return blockRegistry;
        }

        private static List<BlockPreset> LoadJson(string path)
        {
            List<BlockPreset> presets;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                presets = JsonSerializer.Deserialize<List<BlockPreset>>(json);
            }

            return presets;
        }

        private static Block BlockSetup(List<BlockPreset> presets, string name)
        {
            var preset = presets.Find(x => x.name == name);
            var blockCanPlacedOn = new List<int>();
            if (preset.canPlaceOn != "-1")
            {
                var ids = preset.canPlaceOn.Split(';');
                foreach (var id in ids)
                {
                    if (int.TryParse(id, out int parsed))
                    {
                        blockCanPlacedOn.Add(parsed);
                    }
                }
            }

            var temp = new Block(preset.id, preset.tileId, blockCanPlacedOn.ToArray(), preset.name, preset.miningLevel,
                preset.richness, preset.foundationId, preset.blockType, preset.harvestTime, preset.harvestable,preset.dropId, preset.dropCount,
                preset.canPlaceOn != "-1",
                preset.canStepOn);
            return temp;
        }
    }

    public class BlockPreset
    {
        public string name { get; set; }
        public int id { get; set; }
        public int tileId { get; set; }
        public bool canStepOn { get; set; }
        public bool harvestable { get; set; }
        public int harvestTime { get; set; }
        public int blockType { get; set; }
        public int foundationId { get; set; }
        public int richness { get; set; }
        public int miningLevel { get; set; }

        public int dropId { get; set; }

        public int dropCount { get; set; }
        public string canPlaceOn { get; set; }
    }
}