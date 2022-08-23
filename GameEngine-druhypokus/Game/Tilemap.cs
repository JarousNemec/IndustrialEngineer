using System.Runtime.CompilerServices;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Blocks;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner
{
    public class Tilemap : Drawable
    {
        private Texture m_tileset = new Texture("tileset3.png");
        private VertexArray m_vertices = new VertexArray();

        public void load(Vector2u tileSize, Block[,] tiles, uint width, uint height)
        {
            m_vertices.PrimitiveType = PrimitiveType.Quads;
            // resize the vertex array to fit the level size
            m_vertices.Resize(width * height * 4);

            // populate the vertex array, with one quad per tile
            for (uint i = 0; i < width; ++i)
            {
                for (uint j = 0; j < height; ++j)
                {
                    // get the current tile number
                    //int tileNumber = tiles[i + j * width];
                    int tileNumber = tiles[i, j].TileId;
                    // find its position in the tileset texture
                    long tu = tileNumber % (m_tileset.Size.X / tileSize.X);
                    long tv = tileNumber / (m_tileset.Size.X / tileSize.X);

                    // get a pointer to the current tile's quad
                    uint index = (i + j * width) * 4;

                    // define its 4 corners
                    m_vertices[index + 0] = new Vertex(new Vector2f(i * tileSize.X, j * tileSize.Y),
                        new Vector2f(tu * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 1] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y),
                        new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                    m_vertices[index + 2] = new Vertex(new Vector2f((i + 1) * tileSize.X, (j + 1) * tileSize.Y),
                        new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y));
                    m_vertices[index + 3] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y),
                        new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Texture = m_tileset;

            // draw the vertex array
            target.Draw(m_vertices, states);
        }
    }
}