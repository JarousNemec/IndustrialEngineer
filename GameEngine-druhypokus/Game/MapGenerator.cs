using System;
using SFML.System;

namespace GameEngine_druhypokus
{
    public class MapGenerator
    {
        public int[] Generate(int size, int seed)
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

            int[,] map2d = Parse(noiseData);
            int[] map = ConvertArray(map2d);
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

        private int[,] Parse(float[,] map)
        {
            int[,] parsedMap = new int[map.GetLength(0),map.GetLength(1)];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    
                    var temp = (int)Math.Abs(Math.Round(map[y, x], 1) * 10);
                    if (temp > 7)
                    {
                        parsedMap[y, x] = 0;
                        continue;
                    }
                    
                    if (temp == 0)
                    {
                        parsedMap[y, x] = 1;
                        continue;
                    }
                    if (temp == 2)
                    {
                        parsedMap[y, x] = 8;
                        continue;
                    }
                    
                    if (temp == 3)
                    {
                        parsedMap[y, x] = 0;
                        continue;
                    }
                    if (temp > 3)
                    {
                        parsedMap[y, x] = 2;
                        continue;
                    }
                    parsedMap[y, x] = temp;
                    //parsedMap[y, x] = 9;
                }
            }

            return parsedMap;
        }

        private int[] ConvertArray(int[,] array2d)
        {
            var array1d = new int[array2d.Length];
            int index = 0;
            for (int i = 0; i < array2d.GetLength(0); i++)
            {
                for (int j = 0; j < array2d.GetLength(1); j++)
                {
                    array1d[index++] = array2d[i, j];
                }
            }

            return array1d;
        }
    }
}