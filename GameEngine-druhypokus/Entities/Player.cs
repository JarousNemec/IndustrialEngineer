using System.Drawing;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.GameEntities
{
    public class Player : Entity
    {
        public Player(Sprite baseSprite):base(baseSprite)
        {
        }

        public void Draw(RenderWindow window, View view)
        {
            float px = view.Center.X - (BaseSprite.Texture.Size.X / 2);
            float py = view.Center.Y - (BaseSprite.Texture.Size.Y / 2);
            BaseSprite.Position = new Vector2f(px, py);
            BaseSprite.Scale = new Vector2f(1, 1);
            window.Draw(BaseSprite);
        }

        public void SetPosition(float x, float y)
        {
            SetX(x);
            SetY(y);
        }

        public string PrintPosition()
        {
            return $"X: {GetX()}, Y: {GetY()}";
        }

        public void Move(float xStep, float yStep)
        {
            SetX(GetX() + xStep);
            SetY(GetY() + yStep);
        }
    }
}