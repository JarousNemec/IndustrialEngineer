using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.GameEntities
{
    public class Cursor : Entity
    {
        public Player Player { get; set; }
        public ProgressBar _progressBar;

        public Cursor(Sprite sprite, Player player, Sprite[] progressBarStates) : base(sprite)
        {
            Player = player;
            _progressBar = new ProgressBar(progressBarStates);
        }

        public string Draw(RenderWindow window, Vector2i pos)
        {
            Sprite.Position = new Vector2f(pos.X, pos.Y);
            Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Sprite);
            return pos.ToString();
        }

        public Vector2i GetPosition(RenderWindow window, View view, int tileSize, int zoomed, int minzoom,
            Vector2i mouse)
        {
            // half of both window dimensions
            int halfWindowX = (int)(window.Size.X / 2);
            int halfWindowY = (int)(window.Size.Y / 2);
            
            // inverted value of zoom for calculate resolution of displayed blocks
            int invertedZoomed = minzoom - zoomed;
            
            // calculate resolution of the blocks with current zoom
            int resolution = MathSequence(16, invertedZoomed);
            
            // get player coordinates
            int px = (int)Player.GetX();
            int py = (int)Player.GetY();
            
            // calculate how many pixels stands player from the upper left corner of the block he stands
            int oversizeX = (int)((int)Player.GetX() * resolution - view.Center.X / tileSize * resolution);
            int oversizeY = (int)((int)Player.GetY() * resolution - view.Center.Y / tileSize * resolution);
            
            // calculate how long is cursor from the center of window that means from the player
            int tpx = (mouse.X - halfWindowX - oversizeX) / resolution;
            int tpy = (mouse.Y - halfWindowY - oversizeY) / resolution;
            
            //quickfix problem with window half edges
            if (mouse.X <= halfWindowX+oversizeX)
                tpx -= 1;
            if (mouse.Y <= halfWindowY+oversizeY)
                tpy -= 1;
            
            tpx *= tileSize;
            tpy *= tileSize;
            
            // get position of the block under the cursor in pixels 
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
            
            // add chunk corrections to get the position of the block in map array
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