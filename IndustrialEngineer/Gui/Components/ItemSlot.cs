using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemSlot : GuiComponent
    {
        public Item Item { get; set; }
        public int Count { get; set; }
        public bool IsSelected { get; set; }

        private Sprite _selectedSprite;

        private Label _label;

        public ItemSlot(Sprite sprite, Sprite selectedSprite) : base(sprite)
        {
            _label = new Label(null,8,"",GameData.Font);
            _childComponents.Add(_label);
            _selectedSprite = selectedSprite;
            Item = null;
            Count = 0;
            IsSelected = false;
        }

        public bool AddItem(Item item, int count = 1)
        {
            if (Item == null)
            {
                Item = item;
                Count = count;
                _label.Text.DisplayedString = Count.ToString();
                return true;
            }

            if (Item.Id == item.Id)
            {
                Count += count;

                _label.Text.DisplayedString = Count.ToString();
                return true;
            }

            return false;
        }

        public ItemTransportPacket RemoveItem()
        {
            var packet = new ItemTransportPacket(Count, Item);
            Item = null;
            Count = 0;
            return packet;
        }

        public override void Draw(RenderWindow window, Zoom zoom)
        {
            if (!IsSelected)
            {
                base.Draw(window, zoom);
            }
            else
            {
                _selectedSprite.Position = new Vector2f(base.DisplayingX, base.DisplayingY);
                _selectedSprite.Scale = new Vector2f(zoom.FlippedZoomed, zoom.FlippedZoomed);
                window.Draw(_selectedSprite);
            }


            if (Item == null)
                return;

            foreach (var child in _childComponents)
            {
                child.Draw(window, zoom);
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom, Vector2u slotSize)
        {
            base.ActualizeDisplayingCords(newX, newY);
            
            var textPosX = CalculateTextPosition(zoom, slotSize, out var textPosY);

            _label.ActualizeDisplayingCords(textPosX, textPosY);
        }

        private float CalculateTextPosition(Zoom zoom, Vector2u slotSize, out float textPosY)
        {
            int textPosInSlotX = (int)(slotSize.X + 13 - (_label.Text.DisplayedString.Length - 1) * 8);
            int textPosInSlotY = (int)(slotSize.Y + 6);
            float textPosX = (DisplayingX + textPosInSlotX / zoom.Zoomed);
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