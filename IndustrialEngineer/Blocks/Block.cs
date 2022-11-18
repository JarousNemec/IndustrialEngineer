namespace IndustrialEnginner.Blocks
{
    public class Block
    {
        public bool CanStepOn { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int TileId { get; private set; }
        public int[] BlocksCanPlaceOn { get; private set; }
        public bool Harvestable { get; private set; }
        public int HarvestTime { get; private set; }
        public int BlockType { get; private set; }
        public int FoundationId { get; private set; }
        public int Richness { get; set; }
        public int MiningLevel { get; private set; }
        public int DropId { get; private set; }
        public int DropCount { get; set; }
        public int OriginalDropCount { get; set; }
        public bool CanPlaceOn { get; private set; }

        public Block(int id,int tileId, int[] blocksCanPlaceOn,string name, int miningLevel, int richness, int foundationId, int blockType, int harvestTime, bool harvestable, int dropId, int dropCount, bool canPlaceOn = true, bool canStepOn = true)
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
    }
}