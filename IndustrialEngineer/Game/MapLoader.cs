using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;

namespace IndustrialEnginner
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
                    // System.IndexOutOfRangeException: Index was outside the bounds of the array.
                    //     at IndustrialEnginner.MapLoader.GetCurrentChunks(Block[,] map, Int32 mapSize, Int32 renderArea, Player player, Int32 chunkSize) in C:\Users\mortar\RiderProjects\IndustrialEngineer\IndustrialEngineer\Game\MapLoader.cs:line 23
                    // at IndustrialEnginner.Game.UpdateMap() in C:\Users\mortar\RiderProjects\IndustrialEngineer\IndustrialEngineer\Game\Game.cs:line 320
                    // at IndustrialEnginner.Game.Update(GameTime gameTime) in C:\Users\mortar\RiderProjects\IndustrialEngineer\IndustrialEngineer\Game\Game.cs:line 416
                    // at IndustrialEnginner.GameLoop.Run() in C:\Users\mortar\RiderProjects\IndustrialEngineer\IndustrialEngineer\GameLoop\GameLoop.cs:line 72
                    // at IndustrialEnginner.Program.Main(String[] args) in C:\Users\mortar\RiderProjects\IndustrialEngineer\IndustrialEngineer\Program.cs:line 10
                }
            }

            return currentChunks;
        }

        public void Update(Player player, int chunkSize)
        {
            middleXChunk = (int)(player.GetX() / chunkSize);
            middleYChunk = (int)(player.GetY() / chunkSize);
            if (middleXChunk < 1)
                middleXChunk = 1;
            if (middleYChunk < 1)
            {
                middleYChunk = 1;
            }
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