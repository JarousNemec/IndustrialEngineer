using System;
using GameEngine_druhypokus.GameEntities;
using SFML.System;

namespace GameEngine_druhypokus
{
    public class MapGenerator
    {
        //public int[] Generate(int size, int seed)
        public Block[,] Generate(int size, int seed)
        {
            FastNoiseLite noise = new FastNoiseLite();
            var type = FastNoiseLite.NoiseType.OpenSimplex2;
            var frequency = 0.02f;
            var octaves = 5;
            var lacunarity = 2f;
            var gain = 0.4f;
            var strength = 0;
            var fractalType = FastNoiseLite.FractalType.FBm;
            SetNoiseProperties(seed, noise, type, frequency, octaves, lacunarity, gain, strength, fractalType);
            // Gather noise data
            float[,] noiseData = new float[size, size];
            GenerateNoiseData(noiseData, noise);

            Block[,] map2d = Parse(noiseData);
            // int[] map = ConvertArray(map2d);
            // return map;
            return map2d;
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

        private static void SetNoiseProperties(int seed, FastNoiseLite noise, FastNoiseLite.NoiseType type, float frequency, int octaves,
            float lacunarity, float gain, int strength, FastNoiseLite.FractalType fractalType)
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

        private Block[,] Parse(float[,] map)
        {
            Block[,] parsedMap = new Block[map.GetLength(0),map.GetLength(1)];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    
                    var temp = (int)Math.Abs(Math.Round(map[y, x], 1) * 10);
                    if (temp > 7)
                    {
                        parsedMap[y, x] = new Block(0);
                        continue;
                    }
                    
                    if (temp == 0)
                    {
                        parsedMap[y, x] = new Block(1, false);
                        continue;
                    }
                    if (temp == 2)
                    {
                        parsedMap[y, x] = new Block(8);
                        continue;
                    }
                    
                    if (temp == 3)
                    {
                        parsedMap[y, x] = new Block(0);
                        continue;
                    }
                    if (temp > 3)
                    {
                        parsedMap[y, x] = new Block(2, false);
                        continue;
                    }
                    parsedMap[y, x] = new Block(1, false);
                    //parsedMap[y, x] = 9;
                }
            }

            return parsedMap;
        }
    }
}