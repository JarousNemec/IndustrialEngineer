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
        private Area _craftingClickArea;
        private Area _inventoryClickArea;
        private Area _horbarClickArea;
        public GuiManager(GameData gameData, View view, ItemRegistry itemRegistry, Window window)
        {
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData);
            _gui.Inventory.Storage[0, 0].AddItem(itemRegistry.Log.Copy());
            CalculateComponentsClickAreas(window);
        }

        private void CalculateComponentsClickAreas(Window window)
        {
            // _inventoryClickArea = new Area();
            // _craftingClickArea = new Area();
            // _horbarClickArea = new Area();
        }
        
        
        public void UpdatePosition(View view, float zoomed)
        {
            if (zoomed == 0)
            {
                zoomed = 0.5f;
            }
            
            _gui.ActualizeComponentsPositions(view, zoomed);
        }
        private float previousZoomed = 0;
        public void DrawGui(RenderWindow window, float zoomed)
        {
            if (zoomed == 0)
            {
                zoomed = 0.5f;
            }
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

        public void ClickOnGuiComponent(Vector2i mousePosition)
        {
            
        }
        
    }
}