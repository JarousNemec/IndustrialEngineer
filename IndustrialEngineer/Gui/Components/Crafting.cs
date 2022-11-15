using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Crafting : GuiComponent
    {
        public ClickGrid ClickGrid { get; set; }
        public Crafting(Sprite sprite) : base(sprite)
        {
            ClickGrid = new ClickGrid();
        }
    }
}