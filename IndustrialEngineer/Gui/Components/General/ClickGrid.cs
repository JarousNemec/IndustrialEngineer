using System.Drawing;
using IndustrialEnginner.Components;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ClickGrid
    {
        public Area ClickArea { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Vector2i? GetCurrentCell(Vector2i mouse)
        {
            var mouseXPosition = mouse.X - ClickArea.LeftUpCorner.X;
            var mouseYPosition = mouse.Y - ClickArea.LeftUpCorner.Y;
            if (mouseXPosition > 0 && mouseYPosition > 0 && mouseXPosition < ClickArea.RightDownCorner.X && mouseYPosition < ClickArea.RightDownCorner.X)
            {
                var slotSizeX = ClickArea.GetWidth() / Columns;
                var slotSizeY = ClickArea.GetHeight() / Rows;
                int column = mouseXPosition / (slotSizeX);
                int row = mouseYPosition / (slotSizeY);
                return new Vector2i(column>=Columns?Columns-1:column, row>=Rows?Rows-1:row);
            }

            return null;
        }
    }
}