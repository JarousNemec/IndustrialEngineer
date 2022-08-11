using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus.GameEntities
{
    public class Cursor : Entity
    {
        public Player Player { get; set; }

        public Cursor(Sprite sprite, Player player) : base(sprite)
        {
            Player = player;
        }

        public string Draw(RenderWindow window, Vector2i pos)
        {
            Sprite.Position = new Vector2f(pos.X, pos.Y);
            Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Sprite);
            return pos.ToString();
            //return resolution.ToString();
        }

        public Vector2i GetPosition(RenderWindow window, View view, int tileSize, int zoomed, int minzoom,
            Vector2i mouse)
        {
            int halfWindowX = (int)(window.Size.X / 2);
            int halfWindowY = (int)(window.Size.Y / 2);
            int invertedZoomed = minzoom - zoomed;
            int resolution = MathSequence(16, invertedZoomed);
            int px = (int)Player.GetX();
            int py = (int)Player.GetY();
            int oversizeX = (int)((int)Player.GetX() * resolution - view.Center.X / 32 * resolution);
            int oversizeY = (int)((int)Player.GetY() * resolution - view.Center.Y / 32 * resolution);
            int tpx = (mouse.X - halfWindowX - oversizeX) / resolution;
            int tpy = (mouse.Y - halfWindowY - oversizeY) / resolution;
            tpx *= tileSize;
            tpy *= tileSize;
            px = px * tileSize + tpx;
            py = py * tileSize + tpy;
            return new Vector2i(px, py);
        }

        public Vector2i GetWorldPosition(RenderWindow window, View view, int tileSize, int zoomed, int minzoom,
            Vector2i mouse,
            MapLoader mapLoader,
            int chunkSize)
        {
            int chunkCorrectionX = (mapLoader.middleXChunk - 1) * chunkSize;
            int chunkCorrectionY = (mapLoader.middleYChunk - 1) * chunkSize;
            var drawPos = GetPosition(window, view, tileSize, zoomed, minzoom, mouse);
            int px = drawPos.X / tileSize + chunkCorrectionX;
            int py = drawPos.Y / tileSize + chunkCorrectionY;
            return new Vector2i(px, py);
        }

        private int MathSequence(int start, int count)
        {
            int sequence = start;
            for (int i = 1; i < count; i++)
            {
                sequence += sequence;
            }

            return sequence;
        }
    }
}