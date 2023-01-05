using SFML.System;

namespace IndustrialEnginner
{
    public class Mining
    {
        public int Level { get; set; } = 0;
        public bool IsMining { get; set; } = false;
        public int FinishValue { get; set; } = 1;
        public float ActualProgress { get; set; } = 0.1f;

        public Vector2i MiningCoords { get; set; }

        public float Speed { get; set; } = 2f;
    }
}