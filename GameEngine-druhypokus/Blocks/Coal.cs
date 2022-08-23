using IndustrialEnginner.Interfaces;

namespace IndustrialEnginner.Blocks
{
    public class Coal : Block, IBlock
    {
        public static Coal Setup(Block preset)
        {
            return new Coal(preset.Id, preset.TileId, preset.BlocksCanPlaceOn, preset.Name, preset.MiningLevel,
                preset.Richness, preset.FoundationId, preset.BlockType, preset.HarvestTime, preset.Harvestable,
                preset.CanPlaceOn, preset.CanStepOn);
        }


        public Block Copy()
        {
            return new Coal(Id, TileId, BlocksCanPlaceOn, Name, MiningLevel, Richness, FoundationId, BlockType,
                HarvestTime, Harvestable, CanPlaceOn, CanStepOn);
        }

        public Coal(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness,
            int foundationId, int blockType, int harvestTime, bool harvestable, bool canPlaceOn = true,
            bool canStepOn = true) : base(id, tileId, blocksCanPlaceOn, name, miningLevel, richness, foundationId,
            blockType, harvestTime, harvestable, canPlaceOn, canStepOn)
        {
        }
    }
}