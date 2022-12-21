using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class CraftingButton : GuiComponent
    {
        public StorageItem StorageItem { get; set; }
        public bool IsSelected { get; set; }

        private Sprite _selectedSprite;

        private Label _label;
        private PictureBox _pictureBox;
        private GameData _gameData;

        public CraftingButton(Sprite sprite, Sprite selectedSprite, GameData gameData, ComponentType type) : base(sprite,
            type)
        {
            _gameData = gameData;
            _pictureBox = new PictureBox(gameData.GetSprites()["Unknown"]);
            _label = new Label(null, 8, "", GameData.Font);
            _selectedSprite = selectedSprite;
            StorageItem = null;
            IsSelected = false;
        }

        public override void Draw(RenderWindow window, Zoom zoom)
        {
            base.Draw(window, zoom);

            if (StorageItem == null)
                return;

            foreach (var child in _childComponentsToDraw)
            {
                child.Draw(window, zoom);
            }
        }
    }
}