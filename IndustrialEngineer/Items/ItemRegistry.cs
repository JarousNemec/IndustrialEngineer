using System;
using System.Collections.Generic;

namespace IndustrialEnginner.Items
{
    public class ItemRegistry
    {
        public Log Log { get; set; }
        public Stone Stone { get; set; }

        public RawIron RawIron { get; set; }

        public IronIngot IronIngot { get; set; }
        public Drill Drill { get; set; }
        public Furnace Furnace { get; set; }
        public WoodenPlatform WoodenPlatform { get; set; }
        public List<Item> Registry { get; set; }

        public ItemRegistry()
        {
            Registry = new List<Item>();
        }
    }
}