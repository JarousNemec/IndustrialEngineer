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

        public ItemSlot(Sprite sprite, Sprite selectedSprite, float x, float y) : base(sprite, x, y)
        {
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
                return true;
            }

            if (Item == item)
            {
                Count += count;
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

        public override void Draw(RenderWindow window, float zoomed)
        {
            if (!IsSelected)
            {
                base.Draw(window, zoomed);
            }
            else
            {
                _selectedSprite.Position = new Vector2f(base.DisplayingX, base.DisplayingY);
                _selectedSprite.Scale = new Vector2f(zoomed, zoomed);
                window.Draw(_selectedSprite);
            }

            if (Item == null)
                return;
            var itemSprite = Item.Sprite;
            var itemSpritePosX = Sprite.Texture.Size.X / 2 - itemSprite.Texture.Size.X / 2;
            var itemSpritePosY = Sprite.Texture.Size.Y / 2 - itemSprite.Texture.Size.Y / 2;
            itemSprite.Position = new Vector2f(DisplayingX + itemSpritePosX*zoomed, DisplayingY + itemSpritePosY*zoomed);
            itemSprite.Scale = new Vector2f(zoomed, zoomed);
            window.Draw(itemSprite);
        }

        public override void OnClick(Vector2i mouse)
        {
            throw new System.NotImplementedException();
        }
    }
}