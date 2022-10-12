using System.Collections.Generic;

namespace IndustrialEnginner.Items
{
    public class ItemRegistry
    {
        public Log Log { get; set; }
        public List<Item> Registry { get; set; }

        public ItemRegistry()
        {
            Registry = new List<Item>();
        }
    }
}