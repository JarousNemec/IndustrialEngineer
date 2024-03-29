using System;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Gui;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.GameEntities
{
    public class Cursor : GraphicsEntity
    {
        public Player Player { get; set; }
        public ProgressBar _progressBar;
        public PictureBox ActiveItemIcon { get; set; }

        public Cursor(GraphicsEntityProperties cursorEntityProperties,
            GraphicsEntityProperties progressBarEntityProperties, Player player) : base(cursorEntityProperties)
        {
            Player = player;
            _progressBar = new ProgressBar(progressBarEntityProperties);
        }

        public Vector2i Draw(RenderWindow window, Vector2i pos, Zoom zoom, View view, GuiState state)
        {
            if (state == GuiState.GamePlay)
            {
                DrawSelectCursor(window, pos);
                return pos;
            }
            if (ActiveItemIcon != null)
                DrawActiveItemOnCursor(window, Mouse.GetPosition(window), zoom, view);

            return pos;
        }

        private void DrawSelectCursor(RenderWindow window, Vector2i pos)
        {
            Properties.Sprite.Position = new Vector2f(pos.X, pos.Y);
            Properties.Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Properties.Sprite);
        }

        public void SetActiveItemIcon(Item item)
        {
            ActiveItemIcon = new PictureBox(item.Properties.Sprite);
        }

        public void RemoveActiveItemIcon()
        {
            ActiveItemIcon = null;
        }

        private void DrawActiveItemOnCursor(RenderWindow window, Vector2i pos, Zoom zoom, View view)
        {
            ActiveItemIcon.ActualizeDisplayingCords(view.Center.X - view.Size.X / 2 + pos.X / zoom.Zoomed,
                view.Center.Y - view.Size.Y / 2 + pos.Y / zoom.Zoomed);
            ActiveItemIcon.Draw(window, zoom);
        }

        public Vector2i GetPosition(RenderWindow window, View view, int tileSize, float flippedZoomed, float maxZoom,
            Vector2i mouse)
        {
            // half of both window dimensions
            int halfWindowX = (int)(window.Size.X / 2);
            int halfWindowY = (int)(window.Size.Y / 2);

            // inverted value of zoom for calculate resolution of displayed blocks
            float invertedZoomed = maxZoom - flippedZoomed;

            // calculate resolution of the blocks with current zoom
            float resolution = CalculateResolution(16, invertedZoomed);

            // get player coordinates
            int px = (int)Player.Properties.X;
            int py = (int)Player.Properties.Y;

            // calculate how many pixels stands player from the upper left corner of the block he stands
            int oversizeX = (int)(px * resolution - view.Center.X / tileSize * resolution);
            int oversizeY = (int)(py * resolution - view.Center.Y / tileSize * resolution);

            // calculate how long is cursor from the center of window that means from the player
            int tpx = (int)((mouse.X - halfWindowX - oversizeX) / resolution);
            int tpy = (int)((mouse.Y - halfWindowY - oversizeY) / resolution);

            //quickfix problem with window half edges
            if (mouse.X <= halfWindowX + oversizeX)
                tpx -= 1;
            if (mouse.Y <= halfWindowY + oversizeY)
                tpy -= 1;

            tpx *= tileSize;
            tpy *= tileSize;

            // get position of the block under the cursor in pixels 
            px = px * tileSize + tpx;
            py = py * tileSize + tpy;
            return new Vector2i(px, py);
        }

        public Vector2i GetWorldPosition(RenderWindow window, View view, int tileSize, float flippedZoomed,
            float maxZoom,
            Vector2i mouse,
            MapLoader mapLoader,
            int chunkSize, int chunksAroundMiddleChunk)
        {
            int chunkCorrectionX = (mapLoader.middleXChunk - chunksAroundMiddleChunk) * chunkSize;
            int chunkCorrectionY = (mapLoader.middleYChunk - chunksAroundMiddleChunk) * chunkSize;
            var drawPos = GetPosition(window, view, tileSize, flippedZoomed, maxZoom, mouse);

            // add chunk corrections to get the position of the block in map array
            int px = drawPos.X / tileSize + chunkCorrectionX;
            int py = drawPos.Y / tileSize + chunkCorrectionY;
            return new Vector2i(px, py);
        }

        private float CalculateResolution(int start, float count)
        {
            if (count >= 1)
            {
                int sequence = start;
                for (int i = 1; i < count; i++)
                {
                    sequence += sequence;
                }

                return sequence;
            }

            if (count == 0.5)
            {
                return start / 2;
            }

            return start;
        }
    }
}