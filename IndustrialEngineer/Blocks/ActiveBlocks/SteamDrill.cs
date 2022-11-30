using IndustrialEnginner.Gui;

namespace IndustrialEngineer.ActiveBlocks
{
    public class SteamDrill : ActiveBlock
    {
        public SteamDrill(int id, int tileId, int[] blocksCanPlaceOn, string name, int miningLevel, int richness, int foundationId, int blockType, int harvestTime, bool harvestable, int dropId, int dropCount, GuiComponent interactionGui, int[] animationTilesIds, int[] stateTilesIds, bool canPlaceOn = true, bool canStepOn = true) : base(id, tileId, blocksCanPlaceOn, name, miningLevel, richness, foundationId, blockType, harvestTime, harvestable, dropId, dropCount, interactionGui, animationTilesIds, stateTilesIds, canPlaceOn, canStepOn)
        {
        }
    }
}