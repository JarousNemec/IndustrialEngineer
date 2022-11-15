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

        private static uint fontSize = 16;

        private Label _label;

        public ItemSlot(Sprite sprite, Sprite selectedSprite) : base(sprite)
        {
            _label = new Label(null);
            _childComponents.Add(_label);
            _label.Text = new Text("", GameData.Font, fontSize);
            _label.Text.Color = Color.Black;
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

            foreach (var child in _childComponents)
            {
                child.Draw(window, zoomed);
            }
        }

        
        private static float requiredFontSize = 8;
        private static float fontSizeCorrection = requiredFontSize / fontSize;
        private float minfontCellSize = fontSize / 2 * fontSizeCorrection;

        private Vector2f textPos;

        public void ActualizeDisplayingCords(float newX, float newY, float zoomed)
        {
            base.ActualizeDisplayingCords(newX, newY);
            
            var sprite = Sprite;
            var spritePosX = Sprite.Texture.Size.X / 2 - sprite.Texture.Size.X / 2;
            var spritePosY = Sprite.Texture.Size.Y / 2 - sprite.Texture.Size.Y / 2;
            textPos = new Vector2f((16 - minfontCellSize * (Count.ToString().Length - 1)) * zoomed,//todo: remake this calculation
                13 * zoomed);
            _label.ActualizeDisplayingCords(DisplayingX + textPos.X + spritePosX * zoomed+15,DisplayingY + textPos.Y + spritePosY * zoomed+15);
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