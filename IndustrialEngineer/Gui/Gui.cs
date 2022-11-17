using IndustrialEnginner.Components;
using IndustrialEnginner.DataModels;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class Gui
    {
        public Hotbar Hotbar { get; set; }
        public Inventory Inventory { get; set; }
        public Crafting Crafting { get; set; }

        public Vector2i Center { get; set; }

        public Gui(GameData gameData, Window window, Zoom zoom)
        {
            InitializeComponents(gameData);
            CalculateComponentsClickAreas(window, zoom);
        }

        private void InitializeComponents(GameData gameData)
        {
            Hotbar = new Hotbar(gameData.GetSprites()["hotbar"], gameData.GetSprites()["itemslot"],
                gameData.GetSprites()["itemslot_selected"], 1, 9);
            Inventory = new Inventory(gameData.GetSprites()["inventory"], gameData.GetSprites()["itemslot"],
                gameData.GetSprites()["itemslot_selected"], 4, 8);
            Crafting = new Crafting(gameData.GetSprites()["crafting"]);
        }


        private void CalculateComponentsClickAreas(Window window, Zoom zoom)
        {
            CalculateComponentsPositionsInWindow(window, zoom);

            const int hotbarClickAreaMarginX = 5;
            const int hotbarClickAreaMarginY = 5;
            var leftUpCornerHotbar = new Vector2i(hotbarClickAreaMarginX + Hotbar.ComponentPosInWindowX,
                hotbarClickAreaMarginY + Hotbar.ComponentPosInWindowY);
            var rightDownCornerHotbar = new Vector2i(578 + leftUpCornerHotbar.X, 60 + leftUpCornerHotbar.Y);
            Hotbar.ClickGrid.ClickArea = new Area(leftUpCornerHotbar, rightDownCornerHotbar);

            const int inventoryClickAreaMarginX = 17;
            const int inventoryClickAreaMarginY = 20;
            var leftUpCornerInventory = new Vector2i(inventoryClickAreaMarginX + Inventory.ComponentPosInWindowX,
                inventoryClickAreaMarginY + Inventory.ComponentPosInWindowY);
            var rightDownCornerInventory = new Vector2i(443 + leftUpCornerInventory.X, 218 + leftUpCornerInventory.Y);
            Inventory.ClickGrid.ClickArea = new Area(leftUpCornerInventory, rightDownCornerInventory);

            var leftUpCornerCrafting =
                new Vector2i(0 + Crafting.ComponentPosInWindowX, 0 + Crafting.ComponentPosInWindowY);
            var rightDownCornerCrafting = new Vector2i(220 + leftUpCornerCrafting.X, 320 + leftUpCornerCrafting.Y);
            Crafting.ClickGrid.ClickArea = new Area(leftUpCornerCrafting, rightDownCornerCrafting);
        }


        public void ActualizeComponentsPositions(View view, Zoom zoom)
        {
            Hotbar.ActualizeDisplayingCords(view.Center.X - Hotbar.Sprite.Texture.Size.X / 2 * zoom.FlippedZoomed,
                view.Center.Y + view.Size.Y / 2 - (Hotbar.Sprite.Texture.Size.Y - 2) * zoom.FlippedZoomed, zoom);
            Inventory.ActualizeDisplayingCords(
                view.Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 * zoom.FlippedZoomed,
                view.Center.Y - Inventory.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed, zoom, marginX: 6, marginY: 8, marginBetween: -4f);
            Crafting.ActualizeDisplayingCords(
                view.Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 * zoom.FlippedZoomed +
                Inventory.Sprite.Texture.Size.X * zoom.FlippedZoomed, view.Center.Y - Crafting.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed);
        }

        private void CalculateComponentsPositionsInWindow(Window window, Zoom zoom)
        {
            CalculateCenter(window);
            Hotbar.SetPosInWindow((int)(Center.X - Hotbar.Sprite.Texture.Size.X / 2 * zoom.FlippedZoomed),
                (int)(Center.Y + window.Size.Y / 2 - (Hotbar.Sprite.Texture.Size.Y - 2) * zoom.FlippedZoomed));
            Inventory.SetPosInWindow(
                (int)(Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 * zoom.FlippedZoomed),
                (int)(Center.Y - Inventory.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed));
            Crafting.SetPosInWindow(
                (int)(Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 * zoom.FlippedZoomed +
                      Inventory.Sprite.Texture.Size.X * zoom.FlippedZoomed),
                (int)(Center.Y - Crafting.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed));
        }

        private void CalculateCenter(Window window)
        {
            Center = new Vector2i((int)(window.Size.X / 2), (int)(window.Size.Y / 2));
        }

        public void DrawComponents(RenderWindow window, Zoom zoom, GuiState state)
        {
            if (state == GuiState.GamePlay)
            {
                Hotbar.Draw(window, zoom);
            }
            else if (state == GuiState.OpenPlayerInventory)
            {
                Hotbar.Draw(window, zoom);
                Inventory.Draw(window, zoom);
                Crafting.Draw(window, zoom);
            }
        }
    }
}