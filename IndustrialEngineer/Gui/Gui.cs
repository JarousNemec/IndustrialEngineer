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

        private GameData _gameData;

        public Gui(GameData gameData, RenderWindow window, Zoom zoom)
        {
            _gameData = gameData;
            InitializeComponents();
            CalculateComponentsClickAreas(window, zoom, gameData.GetSprite("itemslot").Texture.Size);
            ActualOpenedDialog = _gameData.DialogsRegistry.FurnaceDialog.Copy();
        }

        private void InitializeComponents()
        {
            Hotbar = new Hotbar(_gameData.GetSprite("hotbar"), _gameData.GetSprite("itemslot"),
                _gameData.GetSprite("itemslot_selected"), 1, 9, _gameData);
            Inventory = new Inventory(_gameData.GetSprite("inventory"), _gameData.GetSprite("itemslot"),
                _gameData.GetSprite("itemslot_selected"), 4, 8, _gameData);

            int columns = 4;
            int rowsCount = _gameData.RecipesRegistry.CraftingRecipes.Count / columns;
            int rows = _gameData.RecipesRegistry.CraftingRecipes.Count % columns > 0 ? rowsCount + 1 : rowsCount;

            Crafting = new Crafting(_gameData.GetSprite("crafting"), _gameData.GetSprite("itemslot"),
                _gameData.RecipesRegistry, ComponentType.Crafting, rows, columns);

            _gameData.DialogsRegistry = new DialogsRegistry();
            _gameData.DialogsRegistry.DrillDialog =
                new MachineDialog(
                    new[] { _gameData.GetSprite("drill_dialog"), _gameData.GetSprite("drill_dialog_active") },
                    _gameData,
                    GenerateItemStoragesForDialog(2, _gameData.GetSprite("itemslot"),_gameData.GetSprite("itemslot_selected"),_gameData), new []{new Vector2i(5,57), new Vector2i(65,117)});
            _gameData.DialogsRegistry.FurnaceDialog =
                new MachineDialog(
                    new[] { _gameData.GetSprite("furnace_dialog"), _gameData.GetSprite("furnace_dialog_active") },
                    _gameData, GenerateItemStoragesForDialog(3, _gameData.GetSprite("itemslot"),_gameData.GetSprite("itemslot_selected"),_gameData), new []{new Vector2i(5,89), new Vector2i(50,118),new Vector2i(93,89)});

            
        }

        private ItemStorage[] GenerateItemStoragesForDialog(int count, Sprite itemSlot, Sprite selectedSlot, GameData gameData)
        {
            ItemStorage[] storages = new ItemStorage[count];
            for (int i = 0; i < count; i++)
            {
                storages[i] = new ItemStorage(itemSlot, itemSlot, selectedSlot, 1, 1, gameData);
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
                _gameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowX,
                _gameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowY);
            var drillItemStorageRightDownCorner = new Vector2i(
                (int)(_gameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowX + slotSize.X*2),
                (int)(_gameData.DialogsRegistry.DrillDialog.ItemStorages[0].ComponentPosInWindowY + slotSize.Y*2));
            _gameData.DialogsRegistry.DrillDialog.ItemStorages[0].ClickGrid.ClickArea =
                new Area(drillItemStorageLeftUpCorner, drillItemStorageRightDownCorner);

            drillItemStorageLeftUpCorner = new Vector2i(
                _gameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowX,
                _gameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowY);
            drillItemStorageRightDownCorner = new Vector2i(
                (int)(_gameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowX + slotSize.X*2),
                (int)(_gameData.DialogsRegistry.DrillDialog.ItemStorages[1].ComponentPosInWindowY + slotSize.Y*2));
            _gameData.DialogsRegistry.DrillDialog.ItemStorages[1].ClickGrid.ClickArea =
                new Area(drillItemStorageLeftUpCorner, drillItemStorageRightDownCorner);

            //setting clickareas for all furnace itemStorages
            var furnaceItemStorageLeftUpCorner = new Vector2i(
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowX,
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowY);
            var furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowX + slotSize.X*2),
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ComponentPosInWindowY + slotSize.Y*2));
            _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[0].ClickGrid.ClickArea =
                new Area(furnaceItemStorageLeftUpCorner, furnaceItemStorageRightDownCorner);

            furnaceItemStorageLeftUpCorner = new Vector2i(
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowX,
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowY);
            furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowX + slotSize.X*2),
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ComponentPosInWindowY + slotSize.Y*2));
            _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[1].ClickGrid.ClickArea =
                new Area(furnaceItemStorageLeftUpCorner, furnaceItemStorageRightDownCorner);

            furnaceItemStorageLeftUpCorner = new Vector2i(
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowX,
                _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowY);
            furnaceItemStorageRightDownCorner = new Vector2i(
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowX + slotSize.X*2),
                (int)(_gameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ComponentPosInWindowY + slotSize.Y*2));
            _gameData.DialogsRegistry.FurnaceDialog.ItemStorages[2].ClickGrid.ClickArea =
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

            _gameData.DialogsRegistry.DrillDialog.SetPosInWindow(leftDialogPosX, leftDialogPosY,zoom);
            
            _gameData.DialogsRegistry.FurnaceDialog.SetPosInWindow(leftDialogPosX, leftDialogPosY,zoom);
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