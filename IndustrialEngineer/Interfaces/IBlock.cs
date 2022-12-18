using IndustrialEngineer.Blocks;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;

namespace IndustrialEnginner.Interfaces
{
    public interface IBlock
    {
        Block Copy();
        void PlaceEntity(PlaceableEntity entity);
        void RemoveEntity();
    }
}