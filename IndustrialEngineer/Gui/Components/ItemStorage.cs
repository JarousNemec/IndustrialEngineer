using System.Security.Cryptography.X509Certificates;
using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
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
            int columns, GameData gameData) : base(sprite, ComponentType.Storage)
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
                    Storage[i, j] = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite, gameData, ComponentType.StorageSlot);
                }
            }

            Storage[2, 0].IsSelected = true;
        }

        public override void Draw(RenderWindow window, Zoom zoom)
        {
            base.Draw(window, zoom);
            foreach (var itemSlot in Storage)
            {
                itemSlot.Draw(window, zoom);
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom, float marginX = 0,
            float marginY = 0, float marginBetween = 0)
        {
            base.ActualizeDisplayingCords(newX, newY);

            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    Storage[i, j].ActualizeDisplayingCords(
                        (zoom.FlippedZoomed * i * _itemSlotSize.X) + newX + marginX * zoom.FlippedZoomed + i * marginBetween * zoom.FlippedZoomed,
                        (zoom.FlippedZoomed * j * _itemSlotSize.Y) + newY + marginY * zoom.FlippedZoomed + j * marginBetween * zoom.FlippedZoomed, zoom,_itemSlotSize);
                }
            }
        }
    }
}