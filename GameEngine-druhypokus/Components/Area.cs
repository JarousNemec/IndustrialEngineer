using SFML.System;

namespace IndustrialEnginner.Components
{
    public class Area
    {
        public Vector2i LeftUpCorner { get; private set; }
        public Vector2i RightDownCorner { get; private set; }

        public Area(Vector2i leftUpCorner, Vector2i rightDownCorner)
        {
            LeftUpCorner = leftUpCorner;
            RightDownCorner = rightDownCorner;
        }
    }
}