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

        public Sprite SelectedSprite{ get; set; }

        private Label _label;
        private PictureBox _pictureBox;

        public ItemSlot(Sprite sprite, Sprite selectedSprite, ComponentType type) : base(sprite,
            type)
        {
            _pictureBox = new PictureBox(GameData.Sprites["Unknown"]);
            _label = new Label(null, 8, "", GameData.Font);
            SelectedSprite = selectedSprite;
            StorageItem = null;
            IsSelected = false;
        }

        public ItemSlot Copy()
        {
            ItemSlot slot = new ItemSlot(Sprite, SelectedSprite, ComponentType.StorageSlot);
            slot.SetPosInWindow(ComponentPosInWindowX, ComponentPosInWindowY);
            slot.ActualizeDisplayingCords(DisplayingX, DisplayingY);
            return slot;
        }

        public bool AddItem(StorageItem storageItem)
        {
            if (StorageItem == null)
            {
                StorageItem = storageItem;
                _pictureBox.Sprite = StorageItem.Item.Properties.Sprite;
                _childComponentsToDraw.Add(_pictureBox);
                _childComponentsToDraw.Add(_label);

                _label.Text.DisplayedString = StorageItem.Count.ToString();
                return true;
            }

            if (StorageItem.Item.Properties.Id == storageItem.Item.Properties.Id && StorageItem.Item.Properties.MaxStackCount >= StorageItem.Count+storageItem.Count)
            {
                StorageItem.Count += storageItem.Count;
                _label.Text.DisplayedString = StorageItem.Count.ToString();
                return true;
            }

            return false;
        }

        public bool RemoveItem(int count)
        {
            if (StorageItem == null)
            {
                return false;
            }
            if (count == StorageItem.Count)
            {
                StorageItem = null;
                IsSelected = false;
                _childComponentsToDraw.Clear();
                return true;
            }

            if (count < StorageItem.Count)
            {
                StorageItem.Count -= count;
                _label.Text.DisplayedString = StorageItem.Count.ToString();
                return true;
            }

            return false;
        }
        public override void Draw(RenderWindow window, Zoom zoom)
        {
            base.Draw(window, zoom);
            if (IsSelected)
            {
                SelectedSprite.Position = new Vector2f(base.DisplayingX, base.DisplayingY);
                SelectedSprite.Scale = new Vector2f(zoom.FlippedZoomed, zoom.FlippedZoomed);
                window.Draw(SelectedSprite);
            }
            foreach (var childComponent in _childComponentsToDraw)
            {
                childComponent.Draw(window, zoom);
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
    }
}