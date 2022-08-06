using System.Drawing;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus.GameEntities
{
    public class Player
    {
        public Sprite Sprite { get; private set; }
        public Point Position { get; set; }

        public Player(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void Draw(RenderWindow window,View View)
        {
            var view = View.Center;
            float px = view.X - (Sprite.Texture.Size.X / 2);
            float py = view.Y - (Sprite.Texture.Size.Y / 2);
            Sprite.Position = new Vector2f(px, py);
            Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Sprite);
        }
    }
}