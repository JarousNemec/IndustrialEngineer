using IndustrialEnginner.Components;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class Gui
    {
        public Hotbar Hotbar { get; set; }
        public Inventory Inventory { get; set; }
        public Crafting  Crafting { get; set; }

        public Vector2i Center { get; set; }
        
        public Gui(GameData gameData, Window window, int defaulZoom)
        {
            InitializeComponents(gameData);
            CalculateComponentsClickAreas(window,defaulZoom);
        }

        private void InitializeComponents(GameData gameData)
        {
            Hotbar = new Hotbar(gameData.GetSprites()["hotbar"],gameData.GetSprites()["itemslot"],gameData.GetSprites()["itemslot_selected"], 1,9);
            Inventory = new Inventory(gameData.GetSprites()["inventory"], gameData.GetSprites()["itemslot"],
                gameData.GetSprites()["itemslot_selected"],4,8);
            Crafting = new Crafting(gameData.GetSprites()["crafting"],6,3);
        }
        
        
        private void CalculateComponentsClickAreas(Window window, int defaulZoom)
        {
            CalculateComponentsPositionsInWindow(window,defaulZoom);
            
            const int hotbarCaMargin = 5;
            var leftUpCornerHotbar = new Vector2i(hotbarCaMargin+Hotbar.ComponentPosInWindowX, hotbarCaMargin+Hotbar.ComponentPosInWindowY);
            var rightDownCornerHotbar = new Vector2i(578+leftUpCornerHotbar.X,60+leftUpCornerHotbar.Y);
            Hotbar.SlotGrid.ClickArea = new Area(leftUpCornerHotbar, rightDownCornerHotbar);

            const int inventoryCaMargin = 15;
            var leftUpCornerInventory = new Vector2i(inventoryCaMargin+Inventory.ComponentPosInWindowX,inventoryCaMargin+Inventory.ComponentPosInWindowY);
            var rightDownCornerInventory = new Vector2i(443+leftUpCornerInventory.X,218+leftUpCornerInventory.Y);
            Inventory.SlotGrid.ClickArea = new Area(leftUpCornerInventory, rightDownCornerInventory);

            var leftUpCornerCrafting = new Vector2i(0+Crafting.ComponentPosInWindowX,0+Crafting.ComponentPosInWindowY);
            var rightDownCornerCrafting = new Vector2i(220+leftUpCornerCrafting.X, 320+leftUpCornerCrafting.Y);
            Crafting.SlotGrid.ClickArea = new Area(leftUpCornerCrafting, rightDownCornerCrafting);
       }

        
        public void ActualizeComponentsPositions(View view, float zoomed)
        {
            Hotbar.ActualizeDisplayingCords(view.Center.X - Hotbar.Sprite.Texture.Size.X/2*zoomed,view.Center.Y+view.Size.Y/2-(Hotbar.Sprite.Texture.Size.Y-2)*zoomed, zoomed);
            Inventory.ActualizeDisplayingCords(view.Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed, view.Center.Y-Inventory.Sprite.Texture.Size.Y/2*zoomed, zoomed);
            Crafting.ActualizeDisplayingCords(view.Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed+Inventory.Sprite.Texture.Size.X*zoomed, view.Center.Y-Crafting.Sprite.Texture.Size.Y/2*zoomed, zoomed);
        }

        private void CalculateComponentsPositionsInWindow(Window window, float zoomed)
        {  
            CalculateCenter(window);
            Hotbar.SetPosInWindow((int)(Center.X - Hotbar.Sprite.Texture.Size.X/2*zoomed),(int)(Center.Y+window.Size.Y/2-(Hotbar.Sprite.Texture.Size.Y-2)*zoomed));
            Inventory.SetPosInWindow((int)(Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed), (int)(Center.Y-Inventory.Sprite.Texture.Size.Y/2*zoomed));
            Crafting.SetPosInWindow((int)(Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed+Inventory.Sprite.Texture.Size.X*zoomed), (int)(Center.Y-Crafting.Sprite.Texture.Size.Y/2*zoomed));
        }

        private void CalculateCenter(Window window)
        {
            Center = new Vector2i((int)(window.Size.X / 2),(int)(window.Size.Y / 2));
        }
        
        
        public void DrawComponents(RenderWindow window, float zoomed, GuiState state)
        {
            if (state == GuiState.GamePlay)
            {
                Hotbar.Draw(window, zoomed);
            }
            else if (state == GuiState.OpenPlayerInventory)
            {
                Hotbar.Draw(window, zoomed);
                Inventory.Draw(window, zoomed);
                Crafting.Draw(window, zoomed);
            }
        }
    }
}