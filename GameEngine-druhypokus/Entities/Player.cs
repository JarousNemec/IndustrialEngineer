using System.Drawing;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus.GameEntities
{
    public class Player : Entity
    {
        public Player(Sprite sprite):base(sprite)
        {
        }

        public void Draw(RenderWindow window, View view)
        {
            float px = view.Center.X - (Sprite.Texture.Size.X / 2);
            float py = view.Center.Y - (Sprite.Texture.Size.Y / 2);
            Sprite.Position = new Vector2f(px, py);
            Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Sprite);
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