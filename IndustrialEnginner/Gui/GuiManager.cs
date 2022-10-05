using IndustrialEnginner.Components;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class GuiManager
    {
        private Gui _gui;
        private GuiState _state;

        public GuiManager(GameData gameData, View view, ItemRegistry itemRegistry, Window window, int defaultZoom)
        {
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData);
            _gui.Inventory.Storage[0, 0].AddItem(itemRegistry.Log.Copy());
            CalculateComponentsClickAreas(window, defaultZoom);
        }

        public Gui GetGui()
        {
            return _gui;
        }

        private void CalculateComponentsClickAreas(Window window, int defaultZoom)
        {
             var leftUpCornerHotbar = new Vector2i((int)(window.Size.X / 2 - _gui.Hotbar.Sprite.Texture.Size.X),
                 (int)(window.Size.Y - _gui.Hotbar.Sprite.Texture.Size.Y*2));
             var rightDownCornerHotbar = new Vector2i((int)(window.Size.X / 2 + _gui.Hotbar.Sprite.Texture.Size.X),
                 (int)(window.Size.Y));
             _gui.Hotbar.ClickArea = new Area(leftUpCornerHotbar, rightDownCornerHotbar);
             
             var leftUpCornerInventory =
                 new Vector2i(
                     (int)(window.Size.X / 2 -
                           (_gui.Inventory.Sprite.Texture.Size.X + _gui.Crafting.Sprite.Texture.Size.X)),
                     (int)(window.Size.Y / 2 - _gui.Inventory.Sprite.Texture.Size.Y));
             var rightDownCornerInventory =
                 new Vector2i((int)(leftUpCornerInventory.X + _gui.Inventory.Sprite.Texture.Size.X*2),
                     (int)(leftUpCornerInventory.Y + _gui.Inventory.Sprite.Texture.Size.Y*2));
             _gui.Inventory.ClickArea = new Area(leftUpCornerInventory, rightDownCornerInventory);
             
             var leftUpCornerCrafting = new Vector2i(
                 (int)((window.Size.X / 2 -
                        (_gui.Inventory.Sprite.Texture.Size.X + _gui.Crafting.Sprite.Texture.Size.X)) +
                       _gui.Inventory.Sprite.Texture.Size.X*2),
                 (int)(window.Size.Y / 2 - _gui.Crafting.Sprite.Texture.Size.Y));
             var rightDownCornerCrafting =
                 new Vector2i((int)(leftUpCornerCrafting.X + _gui.Crafting.Sprite.Texture.Size.X*2),
                     (int)(leftUpCornerCrafting.Y + _gui.Crafting.Sprite.Texture.Size.Y*2));
             _gui.Crafting.ClickArea = new Area(leftUpCornerCrafting, rightDownCornerCrafting);
        }
        public void UpdatePosition(View view, float zoomed)
        {
            _gui.ActualizeComponentsPositions(view, zoomed);
        }

        public void DrawGui(RenderWindow window, float zoomed)
        {
            _gui.DrawComponents(window, zoomed, _state);
        }

        public void OpenOrClosePlayerInventory()
        {
            if (_state == GuiState.GamePlay)
            {
                _state = GuiState.OpenPlayerInventory;
            }
            else
            {
                _state = GuiState.GamePlay;
            }
        }

        public GuiState GetGuiState()
        {
            return _state;
        }

        public string ClickOnGuiComponent(Vector2i mousePosition, RenderWindow window)
        {
            if (IsPointInArea(mousePosition, _gui.Hotbar.ClickArea.LeftUpCorner, _gui.Hotbar.ClickArea.RightDownCorner))
            {
                _gui.Hotbar.OnClick(mousePosition);
                return "hotbar";
            }if (IsPointInArea(mousePosition, _gui.Inventory.ClickArea.LeftUpCorner, _gui.Inventory.ClickArea.RightDownCorner))
            {
                _gui.Inventory.OnClick(mousePosition);
                return "inventory";
            }if (IsPointInArea(mousePosition, _gui.Crafting.ClickArea.LeftUpCorner, _gui.Crafting.ClickArea.RightDownCorner))
            {
                _gui.Crafting.OnClick(mousePosition);
                return "crafting";
            }
            
            return "other";
        }

        private bool IsPointInArea(Vector2i point, Vector2i leftUpCorner, Vector2i rightDownCorner)
        {
            return leftUpCorner.X < point.X && rightDownCorner.X > point.X && leftUpCorner.Y < point.Y &&
                   rightDownCorner.Y > point.Y;
        }
    }
}