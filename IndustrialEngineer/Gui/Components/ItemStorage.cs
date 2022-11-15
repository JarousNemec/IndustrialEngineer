using System.Security.Cryptography.X509Certificates;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemStorage : GuiComponent
    {
        public ItemSlot[,] Storage { get; set; }
        private Vector2u _itemSlotSize;
        public ClickGrid ClickGrid { get; set; }

        public ItemStorage(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows,
            int columns) : base(sprite)
        {
            _itemSlotSize = itemSlotSprite.Texture.Size;

            ClickGrid = new ClickGrid();
            ClickGrid.Rows = rows;
            ClickGrid.Columns = columns;

            Storage = new ItemSlot[columns, rows];
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    Storage[i, j] = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite);
                }
            }

            Storage[2, 0].IsSelected = true;
        }

        public override void Draw(RenderWindow window, float zoomed)
        {
            base.Draw(window, zoomed);
            foreach (var itemSlot in Storage)
            {
                itemSlot.Draw(window, zoomed);
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, float zoomed, float marginX = 0,
            float marginY = 0, float marginBetween = 0)
        {
            base.ActualizeDisplayingCords(newX, newY);

            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    Storage[i, j].ActualizeDisplayingCords(
                        (zoomed * i * _itemSlotSize.X) + newX + marginX * zoomed + i * marginBetween * zoomed,
                        (zoomed * j * _itemSlotSize.Y) + newY + marginY * zoomed + j * marginBetween * zoomed, zoomed);
                }
            }
        }
    }
}