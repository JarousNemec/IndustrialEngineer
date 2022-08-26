using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class GuiManager
    {
        private Gui _gui;
        public GuiManager(GameData gameData, View view, ItemRegistry itemRegistry)
        {
            _gui = new Gui(gameData, view);
            _gui.Inventory.Storage[0, 0].AddItem(itemRegistry.Log.Copy());
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
            _gui.DrawComponents(window, zoomed);
        }
        
    }
}