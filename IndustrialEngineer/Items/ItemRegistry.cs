using System;
using System.Collections.Generic;

namespace IndustrialEnginner.Items
{
    public class ItemRegistry
    {
        public Log Log { get; set; }
        public Stone Stone { get; set; }

        public RawIron RawIron { get; set; }
        public List<Item> Registry { get; set; }

        public ItemRegistry()
        {
            Registry = new List<Item>();
        }
    }
}