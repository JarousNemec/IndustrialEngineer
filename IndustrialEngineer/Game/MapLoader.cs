using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;

namespace IndustrialEnginner
{
    public class MapLoader
    {
        private int _chunksAroundMiddleChunk;
        private int _defaultMiddleChunk;
        public int middleXChunk;
        public int middleYChunk;

        public MapLoader(int defaultMiddleChunk, int chunksAroundMiddleChunk)
        {
            _defaultMiddleChunk = defaultMiddleChunk;
            _chunksAroundMiddleChunk = chunksAroundMiddleChunk;
            middleXChunk = defaultMiddleChunk;
            middleYChunk = defaultMiddleChunk;
        }
        public Block[,] GetCurrentChunks(Block[,] map, int mapSize, int renderArea, int chunkSize, int chunksInLineCount, int renderChunks)
        {
            if (chunksInLineCount < renderChunks)
                return map;
            Block[,] currentChunks = new Block[renderArea, renderArea];

            CheckBorders(chunksInLineCount, ref middleXChunk, ref middleYChunk);

            for (int y = 0 + (middleXChunk - _chunksAroundMiddleChunk) * chunkSize; y < renderArea + (middleXChunk - _chunksAroundMiddleChunk) * chunkSize; y++)
            {
                for (int x = 0 + (middleYChunk - _chunksAroundMiddleChunk) * chunkSize; x < renderArea + (middleYChunk - _chunksAroundMiddleChunk) * chunkSize; x++)
                {
                    currentChunks[y - (middleXChunk - _chunksAroundMiddleChunk) * chunkSize, x - (middleYChunk - _chunksAroundMiddleChunk) * chunkSize] = map[y, x];
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
            if (middleXChunk < _defaultMiddleChunk)
                middleXChunk = _defaultMiddleChunk;
            if (middleYChunk < _defaultMiddleChunk)
            {
                middleYChunk = _defaultMiddleChunk;
            }
        }

        private void CheckBorders(int chunksInLineCount, ref int middleXChunk, ref int middleYChunk)
        {
            if (middleXChunk < _defaultMiddleChunk)
            {
                middleXChunk = _defaultMiddleChunk;
            }

            if (middleYChunk < _defaultMiddleChunk)
            {
                middleYChunk = _defaultMiddleChunk;
            }

            if (middleXChunk > chunksInLineCount - _chunksAroundMiddleChunk)
            {
                middleXChunk = chunksInLineCount - _chunksAroundMiddleChunk;
            }

            if (middleYChunk > chunksInLineCount - _chunksAroundMiddleChunk)
            {
                middleYChunk = chunksInLineCount - _chunksAroundMiddleChunk;
            }
        }
    }
}