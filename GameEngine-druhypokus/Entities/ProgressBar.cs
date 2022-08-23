using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.GameEntities
{
    public class ProgressBar : Entity
    {
        public string RootTextuteName { get; set; }
        public int StartState { get; set; } = 0;
        public int FinishState { get; set; } = 10;
        

        public void Draw(RenderWindow window,Vector2i pos, int TileSize)
        {
            Sprite.Position = new Vector2f(pos.X, pos.Y);
            Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Sprite);
        }

        public ProgressBar(Sprite sprite) : base(sprite)
        {
        }
    }
}