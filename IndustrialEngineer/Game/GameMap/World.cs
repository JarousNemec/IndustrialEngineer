using IndustrialEngineer.Blocks;
using SFML.System;

namespace IndustrialEnginner
{
    public class World
    {
        public Tilemap RenderedTiles { get; set; }
        public Block[,] Map { get; set; }
        public Block[,] RenderedMapPart { get; set; }
        public int LastStandXChunk = 0;
        public int LastStandYChunk = 0;
        public int ChunkSize { get; set; }
        public int ChunksInLineCount { get; set; }
        public int MapSize { get; set; }
        public int TileSize { get; set; }
        public int RenderChunks { get; set; }
        public int RenderArea { get; set; }
        public int ChunksAroundMiddleChunks { get; set; }

        public WorldManager Manager { get; set; }

        public World(int chunkSize, int chunksInLineCount, int tileSize, int renderChunks)
        {
            ChunkSize = chunkSize;
            ChunksInLineCount = chunksInLineCount;
            TileSize = tileSize;
            RenderChunks = renderChunks;
            MapSize = chunkSize * chunksInLineCount;
            RenderArea = chunkSize * renderChunks;
            ChunksAroundMiddleChunks = (renderChunks - renderChunks % 2) / 2;
        }

        
    }
}