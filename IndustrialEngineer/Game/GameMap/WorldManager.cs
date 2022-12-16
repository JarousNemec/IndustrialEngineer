using System;
using System.Collections.Generic;
using IndustrialEngineer.Blocks;
using IndustrialEnginner.GameEntities;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner
{
    public class WorldManager
    {
        public MapLoader MapLoader;
        private World _world;
        private BlockRegistry _blockRegistry;
        private List<PlaceableEntity> _renderedEntities;
        private Vector2i _renderedAreaCorrections;

        public WorldManager(World world, BlockRegistry blockRegistry)
        {
            _world = world;
            _blockRegistry = blockRegistry;
            _renderedEntities = new List<PlaceableEntity>();
            _renderedAreaCorrections = new Vector2i();
        }

        public void Initialize()
        {
            InitializeWorld();
            InitializeRenderedTiles();
        }
        
        
        private void InitializeWorld()
        {
            Random r = new Random();
            var generator = new MapGenerator(_blockRegistry);
            MapLoader = new MapLoader(_world.ChunksAroundMiddleChunks, _world.ChunksAroundMiddleChunks);
            if (_world.RenderArea > _world.MapSize)
            {
                _world.RenderArea = _world.MapSize;
            }

            _world.LastStandXChunk = MapLoader.middleXChunk;
            _world.LastStandYChunk = MapLoader.middleYChunk;
            _world.Map = generator.Generate(_world.MapSize, r.Next(1, 99999999));
            _world.RenderedMapPart = MapLoader.GetCurrentChunks(_world.Map, _world.MapSize, _world.RenderArea, _world.ChunkSize, _world.ChunksInLineCount,
                _world.RenderChunks);
        }
        private void InitializeRenderedTiles()
        {
            _world.RenderedTiles = new Tilemap();
            _world.RenderedTiles.load(new Vector2u((uint)_world.TileSize, (uint)_world.TileSize), _world.RenderedMapPart, (uint)_world.RenderArea, (uint)_world.RenderArea);
        }
        
        public void UpdateMap()
        {
            _world.RenderedMapPart = MapLoader.GetCurrentChunks(_world.Map, _world.MapSize, _world.RenderArea, _world.ChunkSize, _world.ChunksInLineCount,
                _world.RenderChunks);
            _world.RenderedTiles.load(new Vector2u((uint)_world.TileSize, (uint)_world.TileSize), _world.RenderedMapPart, (uint)_world.RenderArea,
                (uint)_world.RenderArea);
            LoadEntitiesForRender();
        }

        private void LoadEntitiesForRender()
        {
            _renderedEntities.Clear();
            for (int y = 0; y < _world.RenderedMapPart.GetLength(0); y++)
            {
                for (int x = 0; x < _world.RenderedMapPart.GetLength(1); x++)
                {
                    if (_world.RenderedMapPart[y, x].Properties.PlacedEntity != null)
                    {
                        _renderedEntities.Add(_world.RenderedMapPart[y, x].Properties.PlacedEntity);
                    }
                }
            }
        }

        public void DrawEntities(RenderWindow window)
        {
            _renderedAreaCorrections.X = (MapLoader.middleXChunk - _world.ChunksAroundMiddleChunks) *
                                         (_world.ChunkSize * _world.TileSize);
            _renderedAreaCorrections.Y = (MapLoader.middleYChunk - _world.ChunksAroundMiddleChunks) *
                                         (_world.ChunkSize * _world.TileSize);
            for (int i = 0; i < _renderedEntities.Count; i++)
            {
                _renderedEntities[i].Draw(window, _renderedAreaCorrections,_world.TileSize);
            }
        }

        public void ActualizeLastStandedChunksValues()
        {
            _world.LastStandXChunk = MapLoader.middleXChunk;
            _world.LastStandYChunk = MapLoader.middleYChunk;
        }
    }
}