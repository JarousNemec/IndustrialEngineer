using IndustrialEngineer.Enums;
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
        public GuiState State { get; set; }
        public MachineDialog ActualOpenedDialog { get; set; }

        public Vector2i Center { get; set; }

        public Gui(RenderWindow window, Zoom zoom)
        {
            InitializeComponents();
            CalculateComponentsClickAreas(window, zoom, GameData.Sprites["itemslot"].Texture.Size);
            ActualOpenedDialog = GameData.DialogsRegistry.FurnaceDialog.Copy();
        }

        private void InitializeComponents()
        {
            Hotbar = new Hotbar(GameData.Sprites["hotbar"], GameData.Sprites["itemslot"],
                GameData.Sprites["itemslot_selected"], 1, 9);
            Inventory = new Inventory(GameData.Sprites["inventory"], GameData.Sprites["itemslot"],
                GameData.Sprites["itemslot_selected"], 4, 8);

            int columns = 4;
            int rowsCount = GameData.RecipesRegistry.CraftingRecipes.Count / columns;
            int rows = GameData.RecipesRegistry.CraftingRecipes.Count % columns > 0 ? rowsCount + 1 : rowsCount;

            Crafting = new Crafting(GameData.Sprites["crafting"], GameData.Sprites["itemslot"],
                GameData.RecipesRegistry, ComponentType.Crafting, rows, columns);

            GameData.DialogsRegistry = new DialogsRegistry();
            GameData.DialogsRegistry.DrillDialog =
                new MachineDialog(
                    new[] { GameData.Sprites["drill_dialog"], GameData.Sprites["drill_dialog_active"] },
                    GenerateItemStoragesForDialog(2, GameData.Sprites["itemslot"],GameData.Sprites["itemslot_selected"]), new []{new Vector2i(5,57), new Vector2i(65,117)},new ProgressBarComponent(GameData.GraphicsEntitiesRegistry.ProgressBar.Sprite,GameData.GraphicsEntitiesRegistry.ProgressBar.States), new Vector2i(45,30));
            GameData.DialogsRegistry.FurnaceDialog =
                new MachineDialog(
                    new[] { GameData.Sprites["furnace_dialog"], GameData.Sprites["furnace_dialog_active"] }, GenerateItemStoragesForDialog(3, GameData.Sprites["itemslot"],GameData.Sprites["itemslot_selected"]), new []{new Vector2i(5,89), new Vector2i(50,118),new Vector2i(93,89)},new ProgressBarComponent(GameData.GraphicsEntitiesRegistry.ProgressBar.Sprite,GameData.GraphicsEntitiesRegistry.ProgressBar.States),new Vector2i(45,30));
        }
        
        private ItemStorage[] GenerateItemStoragesForDialog(int count, Sprite itemSlot, Sprite selectedSlot)
        {
            ItemStorage[] storages = new ItemStorage[count];
            for (int i = 0; i < count; i++)
            {
                storages[i] = new ItemStorage(itemSlot, itemSlot, selectedSlot, 1, 1);
            }

            return storages;
        }


        private void CalculateComponentsClickAreas(RenderWindow window, Zoom zoom, Vector2u slotSize)
        {
            CalculateComponentsPositionsInWindow(window, zoom);

            //setting clickareas for main guiScreens
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

            const int craftingClickAreaMarginX = 17;
            const int craftingClickAreaMarginY = 20;
            var leftUpCornerCrafting = new Vector2i(craftingClickAreaMarginX + Crafting.ComponentPosInWindowX,
                craftingClickAreaMarginY + Crafting.ComponentPosInWindowY);
            var rightDownCornerCrafting = new Vector2i(219 + leftUpCornerCrafting.X, 328 + leftUpCornerCrafting.Y);
            Crafting.ClickGrid.ClickArea = new Area(leftUpCornerCrafting, rightDownCornerCrafting);

            //setting clickareas for all drill itemStorages
            var drillItemStorageLeftUpCorner = new Vector2i(
                GameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowX,
                GameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowY);
            var drillItemStorageRightDownCorner = new Vector2i(
                (int)(GameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowX + slotSize.X*2),
                (int)(GameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowY + slotSize.Y*2));
            GameData.DialogsRegistry.DrillDialog.ItemStorages[0].ClickGrid.ClickArea =
                new Area(drillItemStorageLeftUpCorner, drillItemStorageRightDownCorner);

            drillItemStorageLeftUpCorner = new Vector2i(
                GameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowX,
                GameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowY);
            drillItemStorageRightDownCorner = new Vector2i(
                (int)(GameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowX + slotSize.X*2),
                (int)(GameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowY + slotSize.Y*2));
            GameData.DialogsRegistry.DrillDialog.ItemStorages[1].ClickGrid.ClickArea =
                new Area(drillItemStorageLeftUpCorner, drillItemStorageRightDownCorner);

            //setting clickareas for all furnace itemStorages
            var furnaceItemStorageLeftUpCorner = new Vector2i(
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowX,
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowY);
            var furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowX + slotSize.X*2),
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowY + slotSize.Y*2));
            GameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ClickGrid.ClickArea =
                new Area(furnaceItemStorageLeftUpCorner, furnaceItemStorageRightDownCorner);

            furnaceItemStorageLeftUpCorner = new Vector2i(
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowX,
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowY);
            furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowX + slotSize.X*2),
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowY + slotSize.Y*2));
            GameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ClickGrid.ClickArea =
                new Area(furnaceItemStorageLeftUpCorner, furnaceItemStorageRightDownCorner);

            furnaceItemStorageLeftUpCorner = new Vector2i(
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowX,
                GameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowY);
            furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowX + slotSize.X*2),
                (int)(GameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowY + slotSize.Y*2));
            GameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ClickGrid.ClickArea =
                new Area(furnaceItemStorageLeftUpCorner, furnaceItemStorageRightDownCorner);
        }


        public void ActualizeComponentsPositions(View view, Zoom zoom)
        {
            Hotbar.ActualizeDisplayingCords(view.Center.X - Hotbar.Sprite.Texture.Size.X / 2 * zoom.FlippedZoomed,
                view.Center.Y + view.Size.Y / 2 - (Hotbar.Sprite.Texture.Size.Y - 2) * zoom.FlippedZoomed, zoom);
            Inventory.ActualizeDisplayingCords(
                view.Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 *
                zoom.FlippedZoomed,
                view.Center.Y - Inventory.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed, zoom, marginX: 6, marginY: 8,
                marginBetween: -4f);

            float leftDialogPosX = (view.Center.X -
                                       (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 *
                                       zoom.FlippedZoomed +
                                       Inventory.Sprite.Texture.Size.X * zoom.FlippedZoomed);
            float leftDialogPosY = (view.Center.Y - Crafting.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed);

            Crafting.ActualizeDisplayingCords(leftDialogPosX, leftDialogPosY, zoom, marginX: 6, marginY: 8,
                marginBetween: -4f);

            ActualOpenedDialog.ActualizeDisplayingCords(leftDialogPosX, leftDialogPosY, zoom);
        }

        private void CalculateComponentsPositionsInWindow(Window window, Zoom zoom)
        {
            CalculateCenter(window);
            Hotbar.SetPosInWindow((int)(Center.X - Hotbar.Sprite.Texture.Size.X / 2 * zoom.FlippedZoomed),
                (int)(Center.Y + window.Size.Y / 2 - (Hotbar.Sprite.Texture.Size.Y - 2) * zoom.FlippedZoomed));
            Inventory.SetPosInWindow(
                (int)(Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) / 2 *
                    zoom.FlippedZoomed),
                (int)(Center.Y - Inventory.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed));

            int leftDialogPosX = (int)(Center.X - (Inventory.Sprite.Texture.Size.X + Crafting.Sprite.Texture.Size.X) /
                                       2 * zoom.FlippedZoomed +
                                       Inventory.Sprite.Texture.Size.X * zoom.FlippedZoomed);
            int leftDialogPosY = (int)(Center.Y - Crafting.Sprite.Texture.Size.Y / 2 * zoom.FlippedZoomed);
            Crafting.SetPosInWindow(leftDialogPosX, leftDialogPosY);

            GameData.DialogsRegistry.DrillDialog.SetPosInWindow(leftDialogPosX, leftDialogPosY,zoom);
            
            GameData.DialogsRegistry.FurnaceDialog.SetPosInWindow(leftDialogPosX, leftDialogPosY,zoom);
        }

        private void CalculateCenter(Window window)
        {
            Center = new Vector2i((int)(window.Size.X / 2), (int)(window.Size.Y / 2));
        }

        public void DrawComponents(RenderWindow window, Zoom zoom)
        {
            Hotbar.Draw(window, zoom);
            if (State != GuiState.GamePlay)
                Inventory.Draw(window, zoom);
            if (State == GuiState.OpenPlayerInventory)
            {
                Crafting.Draw(window, zoom);
            }
            else if (State == GuiState.OpenMachineDialog)
            {
                ActualOpenedDialog.Draw(window, zoom);
            }
        }
    }
}