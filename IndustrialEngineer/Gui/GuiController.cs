using IndustrialEnginner.Components;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class GuiController
    {
        private Gui _gui;
        private GuiState _state;

        public GuiController(GameData gameData, View view, ItemRegistry itemRegistry, Window window, int defaultZoom)
        {
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData, window, defaultZoom);
            _gui.Inventory.Storage[0, 0].AddItem(itemRegistry.Log.Copy());
        }

        public Gui GetGui()
        {
            return _gui;
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

        public string ClickOnGuiComponent(Vector2i mousePosition)
        {
            
            if (IsPointInArea(mousePosition, _gui.Hotbar.SlotGrid.ClickArea.LeftUpCorner, _gui.Hotbar.SlotGrid.ClickArea.RightDownCorner))
            {
                var cell = _gui.Hotbar.SlotGrid.GetCurrentCell(mousePosition);
                return cell.ToString();
            }if (IsPointInArea(mousePosition, _gui.Inventory.SlotGrid.ClickArea.LeftUpCorner, _gui.Inventory.SlotGrid.ClickArea.RightDownCorner))
            {
                var cell = _gui.Inventory.SlotGrid.GetCurrentCell(mousePosition);
                return cell.ToString();
                //_gui.Inventory.OnClick(mousePosition);
                //return "inventory";
            }if (IsPointInArea(mousePosition, _gui.Crafting.SlotGrid.ClickArea.LeftUpCorner, _gui.Crafting.SlotGrid.ClickArea.RightDownCorner))
            {
                //_gui.Crafting.OnClick(mousePosition);
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