using System;
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

        public int GetWidth()
        {
            return Math.Abs(RightDownCorner.X - LeftUpCorner.X);
        }
        public int GetHeight()
        {
            return Math.Abs(RightDownCorner.Y - LeftUpCorner.Y);
        }

        public override string ToString()
        {
            return $"leftUp X {LeftUpCorner.X}, Y {LeftUpCorner.Y} : rightDown X {RightDownCorner.X}, Y {RightDownCorner.Y}";
        }
    }
}