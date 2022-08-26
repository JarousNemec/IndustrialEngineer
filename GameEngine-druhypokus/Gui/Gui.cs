using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Gui
    {
        public Hotbar Hotbar { get; set; }
        public Inventory Inventory { get; set; }
        public Crafting  Crafting { get; set; }
        
        public Gui(GameData gameData,View view)
        {
            Hotbar = new Hotbar(gameData.GetSprites()["hotbar"],gameData.GetSprites()["itemslot"],gameData.GetSprites()["itemslot_selected"], new Vector2f(0,0));
            Inventory = new Inventory(gameData.GetSprites()["inventory"], gameData.GetSprites()["itemslot"],
                gameData.GetSprites()["itemslot_selected"], new Vector2f(0, 0));
        }

        
        public void ActualizeComponentsPositions(View view, float zoomed)
        {
            Hotbar.ActualizeDisplayingCords((view.Center.X - Hotbar.Sprite.Texture.Size.X/2*zoomed),(view.Center.Y+view.Size.Y/2-((Hotbar.Sprite.Texture.Size.Y-2)*zoomed)), zoomed);
            Inventory.ActualizeDisplayingCords((view.Center.X - Inventory.Sprite.Texture.Size.X/2*zoomed), view.Center.Y-Inventory.Sprite.Texture.Size.Y/2*zoomed, zoomed);
        }

        public void DrawComponents(RenderWindow window, float zoomed)
        {
            Hotbar.Draw(window, zoomed);
            Inventory.Draw(window, zoomed);
        }
    }
}