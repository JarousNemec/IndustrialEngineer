using IndustrialEnginner.Interfaces;

namespace IndustrialEnginner.Blocks
{
    public class Gold : Block, IBlock
    {
        public static Gold Setup(Block preset)
        {
            return new Gold(preset.Id, preset.TileId, preset.BlocksCanPlaceOn, preset.Name, preset.MiningLevel,
                preset.Richness, preset.FoundationId, preset.BlockType, preset.HarvestTime, preset.Harvestable,
                preset.DropId, preset.DropCount,preset.CanPlaceOn, preset.CanStepOn);
        }


        public Block Copy()
        {
            return new Gold(Id, TileId, BlocksCanPlaceOn,Name,MiningLevel,Richness,FoundationId ,BlockType,HarvestTime,Harvestable,DropId,DropCount,CanPlaceOn, CanStepOn);
        }


        public Gold(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness, int foundationId, int blockType, int harvestTime, bool harvestable, int dropId, int dropCount, bool canPlaceOn = true, bool canStepOn = true) : base(id, tileId, blocksCanPlaceOn, name, miningLevel, richness, foundationId, blockType, harvestTime, harvestable, dropId, dropCount, canPlaceOn, canStepOn)
        {
        }
    }
}