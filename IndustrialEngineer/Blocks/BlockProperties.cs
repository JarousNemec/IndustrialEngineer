using IndustrialEnginner.Enums;
using IndustrialEnginner.GameEntities;

namespace IndustrialEngineer.Blocks
{
    public class BlockProperties
    {
        public bool CanStepOn { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TileId { get; set; }
        public int[] BlocksCanPlaceOn { get; set; }
        public bool Harvestable { get; set; }
        public int HarvestTime { get; set; }
        public BlockType BlockType { get; set; }
        public int FoundationId { get; set; }
        public int Richness { get; set; }
        public int MiningLevel { get; set; }
        public int DropId { get; set; }
        public int DropCount { get; set; }
        public int OriginalDropCount { get; set; }
        public bool CanPlaceOn { get; set; }

        public Building PlacedBuilding { get; set; }

        public BlockProperties(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness,
            int foundationId, BlockType blockType, int harvestTime, bool harvestable, int dropId, int dropCount,
            bool canPlaceOn = true, bool canStepOn = true)
        {
            Id = id;
            TileId = tileId;
            BlocksCanPlaceOn = blocksCanPlaceOn;
            CanPlaceOn = canPlaceOn;
            CanStepOn = canStepOn;
            Name = name;
            MiningLevel = miningLevel;
            Richness = richness;
            FoundationId = foundationId;
            BlockType = blockType;
            HarvestTime = harvestTime;
            Harvestable = harvestable;
            DropId = dropId;
            DropCount = dropCount;
            OriginalDropCount = dropCount;
        }
        
        public BlockProperties Copy()
        {
            return new BlockProperties(Id, TileId, BlocksCanPlaceOn, Name, MiningLevel, Richness, FoundationId,
                BlockType, HarvestTime, Harvestable, DropId, DropCount, CanPlaceOn, CanStepOn);
        }
    }
}