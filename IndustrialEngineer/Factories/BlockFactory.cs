using System.Collections.Generic;
using System.IO;
using IndustrialEngineer.Blocks;
using SFML.Audio;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IndustrialEnginner.Blocks
{
    public class BlockFactory
    {
        public static BlockRegistry LoadBlocks(string path)
        {
            BlockRegistry blockRegistry = new BlockRegistry();
            var properties = LoadJson(path);

            blockRegistry.Coal = new Coal(properties.Find(x => x.Name == "Coal"));
            blockRegistry.Registry.Add(blockRegistry.Coal);

            blockRegistry.Copper = new Copper(properties.Find(x => x.Name == "Copper"));
            blockRegistry.Registry.Add(blockRegistry.Copper);

            blockRegistry.Diamond = new Diamond(properties.Find(x => x.Name == "Diamond"));
            blockRegistry.Registry.Add(blockRegistry.Diamond);

            blockRegistry.Emerald = new Emerald(properties.Find(x => x.Name == "Emerald"));
            blockRegistry.Registry.Add(blockRegistry.Emerald);

            blockRegistry.Gold = new Gold(properties.Find(x => x.Name == "Gold"));
            blockRegistry.Registry.Add(blockRegistry.Gold);

            blockRegistry.Grass = new Grass(properties.Find(x => x.Name == "Grass"));
            blockRegistry.Registry.Add(blockRegistry.Grass);

            blockRegistry.Sand = new Sand(properties.Find(x => x.Name == "Sand"));
            blockRegistry.Registry.Add(blockRegistry.Sand);

            blockRegistry.Rock = new Rock(properties.Find(x => x.Name == "Rock"));
            blockRegistry.Registry.Add(blockRegistry.Rock);

            blockRegistry.Tree = new Tree(properties.Find(x => x.Name == "Tree"));
            blockRegistry.Registry.Add(blockRegistry.Tree);

            blockRegistry.Water = new Water(properties.Find(x => x.Name == "Water"));
            blockRegistry.Registry.Add(blockRegistry.Water);

            blockRegistry.Calculus = new Calculus(properties.Find(x => x.Name == "Calculus"));
            blockRegistry.Registry.Add(blockRegistry.Calculus);

            blockRegistry.IronOre = new IronOre(properties.Find(x => x.Name == "IronOre"));
            blockRegistry.Registry.Add(blockRegistry.IronOre);

            return blockRegistry;
        }

        private static List<BlockProperties> LoadJson(string path)
        {
            List<BlockProperties> propertiesList;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                propertiesList = JsonSerializer.Deserialize<List<BlockProperties>>(json);
            }

            return propertiesList;
        }
    }
}