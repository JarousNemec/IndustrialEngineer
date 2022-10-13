using System.Security.Cryptography.X509Certificates;
using IndustrialEnginner.Components;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class GuiComponent
    {

        public Sprite Sprite { get; set; }
        public float DisplayingX { get; set; }
        public float DisplayingY { get; set; }
        public SlotGrid SlotGrid { get; set; }
        public GuiComponent(Sprite sprite, int rows, int columns)
        {
            Sprite = sprite;
            SlotGrid = new SlotGrid();
            SlotGrid.Rows = rows;
            SlotGrid.Columns = columns;
        }

        public virtual void ActualizeDisplayingCords(float newX, float newY, float zoomed)
        {
            DisplayingX = newX;
            DisplayingY = newY;
        }
        
        public virtual void Draw(RenderWindow window, float zoomed)
        {
            Sprite.Position = new Vector2f(DisplayingX, DisplayingY);
            Sprite.Scale = new Vector2f(zoomed, zoomed);
            window.Draw(Sprite);
        }
    }
}