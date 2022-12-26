using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Interfaces;

namespace IndustrialEngineer.Blocks
{
    public class Block : IBlock
    {
        public BlockProperties Properties { get; set; }
        public BlockProperties OriginalProperties { get; set; }

        public Block(BlockProperties properties)
        {
            Properties = properties;
        }

        public Block Copy()
        {
            return new Block(Properties.Copy());
        }

        public void PlaceEntity(Building entity)
        {
            OriginalProperties = Properties;
            Properties = OriginalProperties.Copy();
            Properties.CanPlaceOn = false;
            Properties.Harvestable = true;
            if (Properties.HarvestTime == 0)
                Properties.HarvestTime = 2;
            Properties.CanStepOn = entity.Properties.CanStepOn;
            Properties.PlacedEntity = entity;
        }

        public void RemoveEntity()
        {
            Properties = OriginalProperties;
        }
    }
}