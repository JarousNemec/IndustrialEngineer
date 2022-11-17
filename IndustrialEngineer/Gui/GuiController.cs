using IndustrialEnginner.Components;
using IndustrialEnginner.DataModels;
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
        private ItemRegistry _registry;

        public GuiController(GameData gameData, View view, ItemRegistry itemRegistry, Window window, Zoom zoom)
        {
            _registry = itemRegistry;
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData, window, zoom);
            _gui.Inventory.Storage[0, 0].AddItem(itemRegistry.Log.Copy());
        }

        public Gui GetGui()
        {
            return _gui;
        }

        
        public void UpdatePosition(View view, Zoom zoom)
        {
            _gui.ActualizeComponentsPositions(view, zoom);
        }

        // private int i = 0;
        public void DrawGui(RenderWindow window, Zoom zoom)
        {
            // i++;
            //     if (i > 1)
            //     {
            //         i = 0;
            //         _gui.Inventory.Storage[0, 0].AddItem(_registry.Log.Copy());
            //     }
            _gui.DrawComponents(window, zoom, _state);
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
            
            if (IsPointInArea(mousePosition, _gui.Hotbar.ClickGrid.ClickArea.LeftUpCorner, _gui.Hotbar.ClickGrid.ClickArea.RightDownCorner))
            {
                var cell = _gui.Hotbar.ClickGrid.GetCurrentCell(mousePosition);
                return cell.ToString();
            }if (IsPointInArea(mousePosition, _gui.Inventory.ClickGrid.ClickArea.LeftUpCorner, _gui.Inventory.ClickGrid.ClickArea.RightDownCorner))
            {
                var cell = _gui.Inventory.ClickGrid.GetCurrentCell(mousePosition);
                return cell.ToString();
                //_gui.Inventory.OnClick(mousePosition);
                //return "inventory";
            }if (IsPointInArea(mousePosition, _gui.Crafting.ClickGrid.ClickArea.LeftUpCorner, _gui.Crafting.ClickGrid.ClickArea.RightDownCorner))
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