using SFML.System;

namespace IndustrialEnginner
{
    public class Crafting
    {
        public bool IsCrafting { get; set; } = false;
        public int FinishValue { get; set; } = 1;
        public float ActualProgress { get; set; } = 0.01f;
        public float Speed { get; set; } = 1f;
    }
}