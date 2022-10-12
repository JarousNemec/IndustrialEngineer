using System.Security.Cryptography.X509Certificates;
using IndustrialEnginner.Components;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class GuiComponent
    {

        public Sprite Sprite { get; set; }
        public float LayoutX { get; set; }
        public float LayoutY { get; set; }
        public float DisplayingX { get; set; }
        public float DisplayingY { get; set; }
        public Area ClickArea { get;set; }
        public GuiComponent(Sprite sprite, float layoutX, float layoutY)
        {
            Sprite = sprite;
            LayoutX = layoutX;
            LayoutY = layoutY;
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

        public virtual void OnClick(Vector2i mouse)
        {
            throw new System.NotImplementedException();
        }
    }
}