using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemSlot : GuiComponent
    {
        public StorageItem StorageItem { get; set; }
        public bool IsSelected { get; set; }

        private Sprite _selectedSprite;

        private Label _label;
        private PictureBox _pictureBox;
        private GameData _gameData;

        public ItemSlot(Sprite sprite, Sprite selectedSprite, GameData gameData, ComponentType type) : base(sprite, type)
        {
            _gameData = gameData;
            _pictureBox = new PictureBox(gameData.GetSprites()["Unknown"]);
            _label = new Label(null, 8, "", GameData.Font);
            _selectedSprite = selectedSprite;
            StorageItem = null;
            IsSelected = false;
        }

        public bool AddItem(StorageItem storageItem)
        {
            if (StorageItem == null)
            {
                StorageItem = storageItem;

                _pictureBox.Sprite = StorageItem.Item.Sprite;
                _childComponentsToDraw.Add(_pictureBox);
                _childComponentsToDraw.Add(_label);

                _label.Text.DisplayedString = StorageItem.Count.ToString();
                return true;
            }

            if (StorageItem.Item.Id == storageItem.Item.Id)
            {
                StorageItem.Count += storageItem.Count;
                _label.Text.DisplayedString = StorageItem.Count.ToString();
                return true;
            }

            return false;
        }

        public void RemoveItem()
        {
            // var packet = new ItemTransportPacket(Count, Item);
            StorageItem = null;
            _childComponentsToDraw.Clear();
            // return packet;
        }

        public override void Draw(RenderWindow window, Zoom zoom)
        {
            base.Draw(window, zoom);
            if (IsSelected)
            {
                _selectedSprite.Position = new Vector2f(base.DisplayingX, base.DisplayingY);
                _selectedSprite.Scale = new Vector2f(zoom.FlippedZoomed, zoom.FlippedZoomed);
                window.Draw(_selectedSprite);
            }

            if (StorageItem == null)
                return;

            foreach (var child in _childComponentsToDraw)
            {
                child.Draw(window, zoom);
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom, Vector2u slotSize)
        {
            base.ActualizeDisplayingCords(newX, newY);
            if (StorageItem == null)
                return;
            
            var textPosX = CalculateTextPosition(zoom, slotSize, out var textPosY);
            _label.ActualizeDisplayingCords(textPosX, textPosY);

            var picturePosX = CalculatePicturePosition(newX, newY, zoom, out var picturePosY);
            _pictureBox.ActualizeDisplayingCords(picturePosX, picturePosY);
        }

        private static float CalculatePicturePosition(float newX, float newY, Zoom zoom, out float picturePosY)
        {
            var picturePosX = newX + 6 * zoom.FlippedZoomed;
            picturePosY = newY + 6 * zoom.FlippedZoomed;
            return picturePosX;
        }

        private float CalculateTextPosition(Zoom zoom, Vector2u slotSize, out float textPosY)
        {
            var textPosInSlotX = (int)(slotSize.X + 13 - (_label.Text.DisplayedString.Length - 1) * 8);
            var textPosInSlotY = (int)(slotSize.Y + 6);
            var textPosX = (DisplayingX + textPosInSlotX / zoom.Zoomed);
            textPosY = (DisplayingY + textPosInSlotY / zoom.Zoomed);
            return textPosX;
        }

        private void DrawItemIconToSlot(RenderWindow window, float zoomed, Sprite itemSprite, uint itemSpritePosX,
            uint itemSpritePosY)
        {
            itemSprite.Position =
                new Vector2f(DisplayingX + itemSpritePosX * zoomed, DisplayingY + itemSpritePosY * zoomed);
            itemSprite.Scale = new Vector2f(zoomed, zoomed);
            window.Draw(itemSprite);
        }
    }
}