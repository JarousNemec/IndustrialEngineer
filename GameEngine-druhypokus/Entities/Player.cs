using System.Drawing;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus.GameEntities
{
    public class Player
    {
        public Sprite Sprite { get; private set; }
        private float X = 0;
        private float Y = 0;
        public Player(Sprite sprite)
        {
            Sprite = sprite;
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
            X = x;
            Y = y;
        }

        public float GetX()
        {
            return X;
        }

        public float GetY()
        {
            return Y;
        }

        public string PrintPosition()
        {
            return $"X: {X}, Y: {Y}";
        }

        public void Move(float xStep, float yStep)
        {
            X += xStep;
            Y += yStep;
        }
    }
}