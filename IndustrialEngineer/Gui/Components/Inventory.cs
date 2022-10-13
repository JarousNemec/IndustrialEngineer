using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Inventory : GuiComponent
    {
        public ItemSlot[,] Storage { get; set; }
        private Vector2u _itemSlotSize;
        public Inventory(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows, int columns) : base(sprite, rows, columns)
        {
            _itemSlotSize = itemSlotSprite.Texture.Size;
            _itemSlotSize.X = _itemSlotSize.X - 4;
            _itemSlotSize.Y = _itemSlotSize.Y - 4;
            Storage = new ItemSlot[8,4];
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    Storage[i,j] = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite,1,1);
                }
            }
        }

        public override void Draw(RenderWindow window, float zoomed)
        {
            base.Draw(window, zoomed);
            foreach (var itemSlot in Storage)
            {
                itemSlot.Draw(window, zoomed);
            }
        }

        public override void ActualizeDisplayingCords(float newX, float newY, float zoomed)
        {
            base.ActualizeDisplayingCords(newX, newY, zoomed);
            
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    Storage[i,j].ActualizeDisplayingCords((zoomed*i * _itemSlotSize.X)+newX+6*zoomed, (zoomed*j * _itemSlotSize.Y)+newY+8*zoomed, zoomed); 
                }
            }
        }
    }
}