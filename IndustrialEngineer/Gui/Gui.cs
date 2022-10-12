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
        
        public Gui(GameData gameData, Window window)
        {
            
            // Hotbar = new Hotbar(gameData.GetSprites()["hotbar"],gameData.GetSprites()["itemslot"],gameData.GetSprites()["itemslot_selected"], new Vector2f(window.Size.X/2 - gameData.GetSprites()["hotbar"].Texture.Size.X/2,window.Size.Y-(gameData.GetSprites()["hotbar"].Texture.Size.Y-2)));
            // Inventory = new Inventory(gameData.GetSprites()["inventory"], gameData.GetSprites()["itemslot"],
            //     gameData.GetSprites()["itemslot_selected"], new Vector2f(window.Size.X/2 - (gameData.GetSprites()["inventory"].Texture.Size.X+gameData.GetSprites()["crafting"].Texture.Size.X)/2, window.Size.Y/2-gameData.GetSprites()["inventory"].Texture.Size.Y/2));
            // Crafting = new Crafting(gameData.GetSprites()["crafting"], window.Size.X/2 - (gameData.GetSprites()["inventory"].Texture.Size.X+gameData.GetSprites()["crafting"].Texture.Size.X)/2+gameData.GetSprites()["inventory"].Texture.Size.X, window.Size.Y/2-gameData.GetSprites()["crafting"].Texture.Size.Y/2);
            Hotbar = new Hotbar(gameData.GetSprites()["hotbar"],gameData.GetSprites()["itemslot"],gameData.GetSprites()["itemslot_selected"]);
            Inventory = new Inventory(gameData.GetSprites()["inventory"], gameData.GetSprites()["itemslot"],
                gameData.GetSprites()["itemslot_selected"]);
            Crafting = new Crafting(gameData.GetSprites()["crafting"]);

        }

        
        public void ActualizeComponentsPositions(View view, float zoomed)
        {
            Hotbar.ActualizeDisplayingCords(view.Center.X - Hotbar.Sprite.Texture.Size.X/2*zoomed,view.Center.Y+view.Size.Y/2-(Hotbar.Sprite.Texture.Size.Y-2)*zoomed, zoomed);
            Inventory.ActualizeDisplayingCords(view.Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed, view.Center.Y-Inventory.Sprite.Texture.Size.Y/2*zoomed, zoomed);
            Crafting.ActualizeDisplayingCords(view.Center.X - (Inventory.Sprite.Texture.Size.X+Crafting.Sprite.Texture.Size.X)/2*zoomed+Inventory.Sprite.Texture.Size.X*zoomed, view.Center.Y-Crafting.Sprite.Texture.Size.Y/2*zoomed, zoomed);
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