namespace IndustrialEnginner
{
    public class Moving
    {
        public float Step { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }

        public Moving(float step)
        {
            Step = step;
        }

        public bool IsMoving()
        {
            return Left || Right || Up || Down;
        }
    }
}