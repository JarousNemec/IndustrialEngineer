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
        private Text countText;
        private static uint fontSize = 16;
        private Vector2f textPos;

        public ItemSlot(Sprite sprite, Sprite selectedSprite, int rows, int columns) : base(sprite, rows, columns)
        {
            countText = new Text("0", GameData.Font, fontSize);
            countText.Color = Color.Black;
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
                countText.DisplayedString = Count.ToString();
                return true;
            }

            if (Item.Id == item.Id)
            {
                Count += count;

                countText.DisplayedString = Count.ToString();
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

            DrawItemIconToSlot(window, zoomed, itemSprite, itemSpritePosX, itemSpritePosY);
            DrawItemCountToSlot(window, zoomed, itemSpritePosX, itemSpritePosY);
        }

        private static float requiredFontSize = 8;
        private static float fontSizeCorrection = requiredFontSize / fontSize;
        private float minfontCellSize = fontSize / 2 * fontSizeCorrection;

        private void DrawItemCountToSlot(RenderWindow window, float zoomed, uint itemSpritePosX, uint itemSpritePosY)
        {
            textPos = new Vector2f((16 - minfontCellSize * (Count.ToString().Length - 1)) * zoomed,
                13 * zoomed);
            countText.Position = new Vector2f(DisplayingX + textPos.X + itemSpritePosX * zoomed,
                DisplayingY + textPos.Y + itemSpritePosY * zoomed);
            countText.Scale = new Vector2f(fontSizeCorrection * zoomed, fontSizeCorrection * zoomed);
            window.Draw(countText);
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