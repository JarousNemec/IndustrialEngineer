using System.Collections.Generic;

namespace IndustrialEnginner.GameEntities
{
    public class BuildingsRegistry
    {
        public Furnace Furnace { get; set; }
        public Drill Drill { get; set; }
        public WoodenPlatform WoodenPlatform { get; set; }
        public List<Building> Registry { get; set; }

        public BuildingsRegistry()
        {
            Registry = new List<Building>();
        }
    }
}