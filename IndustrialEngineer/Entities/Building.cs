using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.GameEntities
{
    public class Building
    {
        public BuildingProperties Properties { get; set; }
        public Building(BuildingProperties properties)
        {
            Properties = properties;
        }

        public Building Copy()
        {
            return new Building(Properties.Copy());
        }

        public void SetPosition(Vector2i pos)
        {
            Properties.X = pos.X;
            Properties.Y = pos.Y;
        }
        
        public void Draw(RenderWindow window, Vector2i renderedAreaCorrections, int tileSize)
        {
            float px = Properties.X*tileSize-renderedAreaCorrections.X;
            float py = Properties.Y*tileSize-renderedAreaCorrections.Y;
            Properties.Sprite.Position = new Vector2f(px, py);
            Properties.Sprite.Scale = new Vector2f(1, 1);
            window.Draw(Properties.Sprite);
        }
        public virtual void Update(float deltaTime, World world){}

    }
}