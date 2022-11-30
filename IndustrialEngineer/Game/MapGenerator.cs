using System;
using IndustrialEngineer.Blocks;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.Interfaces;
using SFML.System;

namespace IndustrialEnginner
{
    public class MapGenerator
    {
        private BlockRegistry _blockRegistry;

        public MapGenerator(BlockRegistry blockRegistry)
        {
            _blockRegistry = blockRegistry;
        }

        public Block[,] Generate(int size, int seed)
        {
            FastNoiseLite noise = new FastNoiseLite();

            #region Base of the world

            var type = FastNoiseLite.NoiseType.OpenSimplex2;
            float frequency = 0.02f;
            int octaves = 5;
            float lacunarity = 2f;
            float gain = 0.4f;
            float strength = 0;
            var fractalType = FastNoiseLite.FractalType.FBm;
            SetNoiseProperties(seed, noise, type, frequency, octaves, lacunarity, gain, strength, fractalType);
            Block[,] map = GenerateBasicTerrain(size, noise);

            #endregion

            #region Forests
            type = FastNoiseLite.NoiseType.Perlin;
            frequency = 0.05f;
            octaves = 5;
            lacunarity = 2f;
            gain = 0.4f;
            strength = 0;
            fractalType = FastNoiseLite.FractalType.FBm;
            SetNoiseProperties(seed, noise, type, frequency, octaves, lacunarity, gain, strength, fractalType);
            GenerateForests(map, noise);
            #endregion
            
            #region IronOre
            type = FastNoiseLite.NoiseType.Perlin;
            frequency = 0.09f;
            octaves = 5;
            lacunarity = 1f;
            gain = 0.8f;
            strength = 0;
            fractalType = FastNoiseLite.FractalType.FBm;
            SetNoiseProperties(seed, noise, type, frequency, octaves, lacunarity, gain, strength, fractalType);
            GenerateIronOre(map, noise);
            #endregion

            return map;
        }

        private static void GenerateNoiseData(float[,] noiseData, FastNoiseLite noise)
        {
            for (int y = 0; y < noiseData.GetLength(0); y++)
            {
                for (int x = 0; x < noiseData.GetLength(1); x++)
                {
                    noiseData[y, x] = noise.GetNoise(x, y);
                }
            }
        }

        private static void SetNoiseProperties(int seed, FastNoiseLite noise, FastNoiseLite.NoiseType type,
            float frequency, int octaves,
            float lacunarity, float gain, float strength, FastNoiseLite.FractalType fractalType)
        {
            noise.SetNoiseType(type);
            noise.SetFrequency(frequency);
            noise.SetSeed(seed);
            noise.SetFractalOctaves(octaves);
            noise.SetFractalLacunarity(lacunarity);
            noise.SetFractalGain(gain);
            noise.SetFractalWeightedStrength(strength);
            noise.SetFractalType(fractalType);
        }

        private Block[,] GenerateBasicTerrain(int size, FastNoiseLite noise)
        {
            float[,] noiseData = new float[size, size];
            GenerateNoiseData(noiseData, noise);
            Random r = new Random();
            Block[,] generatedMap = new Block[noiseData.GetLength(0), noiseData.GetLength(1)];
            for (int y = 0; y < noiseData.GetLength(0); y++)
            {
                for (int x = 0; x < noiseData.GetLength(1); x++)
                {
                    var temp = (int)Math.Abs(Math.Round(noiseData[y, x], 1) * 10);
                    if (temp == 0)
                    {
                        generatedMap[y, x] = _blockRegistry.Water.Copy();
                        continue;
                    }

                    if (temp == 2)
                    {
                        generatedMap[y, x] = _blockRegistry.Sand.Copy();
                        continue;
                    }

                    if (temp >= 3)
                    {
                        if (r.Next(0,1000)>997)
                        {
                            generatedMap[y, x] = _blockRegistry.Calculus.Copy();
                        }
                        else
                        {
                            generatedMap[y, x] = _blockRegistry.Grass.Copy();
                        }
                        
                        continue;
                    }

                    generatedMap[y, x] = _blockRegistry.Water.Copy();
                }
            }

            return generatedMap;
        }

        private void GenerateForests(Block[,] map, FastNoiseLite noise)
        {
            float[,] noiseData = new float[map.GetLength(0), map.GetLength(1)];
            GenerateNoiseData(noiseData, noise);

            for (int y = 0; y < noiseData.GetLength(0); y++)
            {
                for (int x = 0; x < noiseData.GetLength(1); x++)
                {
                    var temp = (int)Math.Abs(Math.Round(noiseData[y, x], 1) * 10);
                    if (temp > 1 && map[y, x].Id == 0)
                    {
                        map[y, x] = _blockRegistry.Tree.Copy();
                    }
                }
            }
        }

        private void GenerateIronOre(Block[,] map, FastNoiseLite noise)
        {
            float[,] noiseData = new float[map.GetLength(0), map.GetLength(1)];
            GenerateNoiseData(noiseData, noise);

            for (int y = 0; y < noiseData.GetLength(0); y++)
            {
                for (int x = 0; x < noiseData.GetLength(1); x++)
                {
                    var temp = (int)Math.Abs(Math.Round(noiseData[y, x], 1) * 10);
                    
                    if (temp > 4 && map[y, x].Id == 0)
                    {
                        map[y, x] = _blockRegistry.IronOre.Copy();
                    }
                }
            }
        }
    }
}