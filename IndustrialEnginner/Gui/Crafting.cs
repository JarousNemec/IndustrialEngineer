using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Crafting : GuiComponent
    {
        public Crafting(Sprite sprite, float layoutX, float layoutY) : base(sprite, layoutX, layoutY)
        {
        }

        public override void OnClick(Vector2i mouse)
        {
            //throw new System.NotImplementedException();
        }
    }
}