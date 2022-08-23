namespace IndustrialEnginner
{
    public class Moving
    {
        public bool left { get; set; }
        public bool right { get; set; }
        public bool up { get; set; }
        public bool down { get; set; }

        public bool IsMoving()
        {
            return left || right || up || down;
        }
    }
}