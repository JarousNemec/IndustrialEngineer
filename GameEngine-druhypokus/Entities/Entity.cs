using System.Collections.Generic;
using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class Entity
    {
        public Sprite BaseSprite { get; private set; }
        
        private float X = 0;
        private float Y = 0;

        public Entity(Sprite baseSprite)
        {
            BaseSprite = baseSprite;
        }
        
        public float GetX()
        {
            return X;
        }

        public float GetY()
        {
            return Y;
        }

        public void SetX(float x)
        {
            X = x;
        }

        public void SetY(float y)
        {
            Y = y;
        }
    }
}