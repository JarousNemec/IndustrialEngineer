using IndustrialEnginner.Interfaces;

namespace IndustrialEngineer.Blocks
{
    public class Bridge: Block,IBlock
    {
        

        public static Bridge Setup(Block preset)
        {
            return new Bridge(preset.Id, preset.TileId, preset.BlocksCanPlaceOn, preset.Name, preset.MiningLevel,
                preset.Richness, preset.FoundationId, preset.BlockType, preset.HarvestTime, preset.Harvestable,
                preset.DropId, preset.DropCount,preset.CanPlaceOn, preset.CanStepOn);
        }

        

        public Block Copy()
        {
            return new Bridge(Id, TileId, BlocksCanPlaceOn,Name,MiningLevel,Richness,FoundationId ,BlockType,HarvestTime,Harvestable,DropId,DropCount,CanPlaceOn, CanStepOn);
        }


        public Bridge(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness, int foundationId, int blockType, int harvestTime, bool harvestable, int dropId, int dropCount, bool canPlaceOn = true, bool canStepOn = true) : base(id, tileId, blocksCanPlaceOn, name, miningLevel, richness, foundationId, blockType, harvestTime, harvestable, dropId, dropCount, canPlaceOn, canStepOn)
        {
        }
    }
}