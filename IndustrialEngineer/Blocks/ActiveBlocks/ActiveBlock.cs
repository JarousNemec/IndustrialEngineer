using IndustrialEngineer.Blocks;
using IndustrialEnginner.Gui;

namespace IndustrialEngineer.ActiveBlocks
{
    public class ActiveBlock : Block
    {
        public int[] StateTilesIds { get; set; }
        public int[] AnimationTilesIds { get; set; }
        public GuiComponent InteractionGui { get; set; }
        public ActiveBlock(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness, int foundationId, int blockType, int harvestTime, bool harvestable, int dropId, int dropCount,GuiComponent interactionGui,int[] animationTilesIds, int[] stateTilesIds, bool canPlaceOn = true, bool canStepOn = true) : base(id, tileId, blocksCanPlaceOn, name, miningLevel, richness, foundationId, blockType, harvestTime, harvestable, dropId, dropCount, canPlaceOn, canStepOn)
        {
            StateTilesIds = stateTilesIds;
            AnimationTilesIds = animationTilesIds;
            InteractionGui = interactionGui;
        }
    }
}