using System.Drawing;
using IndustrialEnginner.Components;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class SlotGrid
    {
        public Area ClickArea { get;set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Vector2i GetCurrentCell(Vector2i mouse)
        {
            var mouseXPosition = mouse.X - ClickArea.LeftUpCorner.X;
            var mouseYPosition = mouse.Y - ClickArea.LeftUpCorner.Y;
            if (mouseXPosition > 0 && mouseYPosition > 0)
            {
                var slotSizeX = ClickArea.GetWidth() / Columns;
                var slotSizeY = ClickArea.GetHeight() / Rows;
                int clickedSlotX = mouseXPosition / slotSizeX;
                int clickedSlotY = mouseYPosition / slotSizeY;
                return new Vector2i(clickedSlotX, clickedSlotY);
            }

            return new Vector2i(0, 0);
        }
    }
}