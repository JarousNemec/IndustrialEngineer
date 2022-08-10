using GameEngine_druhypokus.GameEntities;

namespace GameEngine_druhypokus
{
    public class MapLoader
    {
        public int middleXChunk = 1;
        public int middleYChunk = 1;

        public Block[,] GetCurrentChunks(Block[,] map, int mapSize, int renderArea, Player player, int chunkSize)
        {
            if (mapSize < renderArea)
                return map;
            Block[,] currentChunks = new Block[renderArea, renderArea];

            CheckBorders(mapSize, ref middleXChunk, ref middleYChunk);

            for (int y = 0 + (middleXChunk - 1) * chunkSize; y < renderArea + (middleXChunk - 1) * chunkSize; y++)
            {
                for (int x = 0 + (middleYChunk - 1) * chunkSize; x < renderArea + (middleYChunk - 1) * chunkSize; x++)
                {
                    currentChunks[y - (middleXChunk - 1) * chunkSize, x - (middleYChunk - 1) * chunkSize] = map[y, x];
                }
            }

            return currentChunks;
        }

        public void Update(Player player, int chunkSize)
        {
            middleXChunk = (int)(player.GetX() / chunkSize);
            middleYChunk = (int)(player.GetY() / chunkSize);
        }

        private static void CheckBorders(int mapSize, ref int middleXChunk, ref int middleYChunk)
        {
            if (middleXChunk < 1)
            {
                middleXChunk = 1;
            }

            if (middleYChunk < 1)
            {
                middleYChunk = 1;
            }

            if (middleXChunk > mapSize - 2)
            {
                middleXChunk = mapSize - 2;
            }

            if (middleYChunk > mapSize - 2)
            {
                middleYChunk = mapSize - 2;
            }
        }
    }
}