using System.Drawing;
using IndustrialEnginner.Gui;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.GameEntities
{
    public class Player : GraphicsEntity
    {
        public Inventory Inventory { get; set; }
        public Hotbar Hotbar { get; set; }
        public Player(GraphicsEntityProperties properties):base(properties)
        {
        }

        public void Draw(RenderWindow window, View view)
        {
            float px = view.Center.X - (Properties.Sprite.Texture.Size.X / 2);
            float py = view.Center.Y - (Properties.Sprite.Texture.Size.Y / 2);
            Properties.Sprite.Position = new Vector2f(px, py);
            Properties.Sprite.Scale = new Vector2f(0.9f, 0.9f);
            window.Draw(Properties.Sprite);
        }

        public void SetPosition(float x, float y)
        {
            Properties.X = x;
            Properties.Y = y;
        }

        public string PrintPosition()
        {
            return $"X: {Properties.X}, Y: {Properties.Y}";
        }

        public void Move(float xStep, float yStep)
        {
            Properties.X += xStep;
            Properties.Y += yStep;
        }
    }
}