using System.Collections.Generic;

namespace IndustrialEnginner.GameEntities
{
    public class PlaceableEntityRegistry
    {
        public Furnace Furnace { get; set; }
        public Drill Drill { get; set; }
        public List<PlaceableEntity> Registry { get; set; }

        public PlaceableEntityRegistry()
        {
            Registry = new List<PlaceableEntity>();
        }
    }
}