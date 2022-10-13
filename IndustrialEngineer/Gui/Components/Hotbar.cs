using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Hotbar : GuiComponent
    {
        public ItemSlot[] Bar { get; set; }
        private Vector2u _itemSlotSize;
        public Hotbar(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows, int columns) : base(sprite, rows, columns)
        {
            _itemSlotSize = itemSlotSprite.Texture.Size;
            Bar = new ItemSlot[9];
            for (int i = 0; i < Bar.Length; i++)
            {
                Bar[i] = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite,1,1);
            }

            Bar[8].IsSelected = true;
        }

        public override void Draw(RenderWindow window, float zoomed)
        {
            base.Draw(window, zoomed);
            foreach (var itemSlot in Bar)
            {
                itemSlot.Draw(window, zoomed);
            }
        }

        public override void ActualizeDisplayingCords(float newX, float newY, float zoomed)
        {
            base.ActualizeDisplayingCords(newX, newY, zoomed);
            for (int i = 0; i < Bar.Length; i++)
            {
                Bar[i].ActualizeDisplayingCords((zoomed*i * _itemSlotSize.X)+newX, newY, zoomed); 
            }
        }
    }
}