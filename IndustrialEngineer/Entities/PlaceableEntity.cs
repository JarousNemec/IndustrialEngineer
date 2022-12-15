namespace IndustrialEnginner.GameEntities
{
    public class PlaceableEntity
    {
        public PlaceableEntityProperties Properties { get; set; }
        public PlaceableEntity(PlaceableEntityProperties properties)
        {
            Properties = properties;
        }
    }
}