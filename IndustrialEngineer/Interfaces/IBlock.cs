using IndustrialEngineer.Blocks;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;

namespace IndustrialEnginner.Interfaces
{
    public interface IBlock
    {
        Block Copy();
        void PlaceEntity(Building entity);
        void RemoveEntity();
    }
}