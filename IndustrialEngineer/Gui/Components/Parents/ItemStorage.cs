using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
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
                    Storage[i, j] = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite, gameData,
                        ComponentType.StorageSlot);
                }
            }
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
                        (zoom.FlippedZoomed * i * _itemSlotSize.X) + newX + marginX * zoom.FlippedZoomed +
                        i * marginBetween * zoom.FlippedZoomed,
                        (zoom.FlippedZoomed * j * _itemSlotSize.Y) + newY + marginY * zoom.FlippedZoomed +
                        j * marginBetween * zoom.FlippedZoomed, zoom, _itemSlotSize);
                }
            }
        }

        public StorageItem AddItem(StorageItem storageItem)
        {
            repeat:
            ItemSlot slotEnableToAddItem = FindEmptyOrWithSameItem(Storage, storageItem.Item);
            if (slotEnableToAddItem == null)
                return storageItem;

            int canMaxAdd = 1;
            if (slotEnableToAddItem.StorageItem == null)
            {
                canMaxAdd = storageItem.Item.Properties.MaxStackCount;
                if (canMaxAdd >= storageItem.Count)
                {
                    if (slotEnableToAddItem.AddItem(storageItem))
                    {
                        return null;
                    }

                    return storageItem;
                }

                slotEnableToAddItem.AddItem(new StorageItem() { Item = storageItem.Item, Count = canMaxAdd });
                storageItem.Count -= canMaxAdd;
                goto repeat;
            }

            canMaxAdd = slotEnableToAddItem.StorageItem.Item.Properties.MaxStackCount - slotEnableToAddItem.StorageItem.Count;
            if (canMaxAdd >= storageItem.Count)
            {
                if (slotEnableToAddItem.AddItem(storageItem))
                {
                    return null;
                }

                return storageItem;
            }

            slotEnableToAddItem.AddItem(new StorageItem() { Item = storageItem.Item, Count = canMaxAdd });
            storageItem.Count -= canMaxAdd;
            goto repeat;
        }

        private ItemSlot FindEmptyOrWithSameItem(ItemSlot[,] storage, Item item)
        {
            for (int x = 0; x < storage.GetLength(1); x++)
            {
                for (int y = 0; y < storage.GetLength(0); y++)
                {
                    if (storage[y, x].StorageItem == null)
                        return storage[y, x];

                    var storageItem = storage[y, x].StorageItem;
                    if (storageItem.Item.Properties.Id == item.Properties.Id && storageItem.Count < storageItem.Item.Properties.MaxStackCount)
                        return storage[y, x];
                }
            }

            return null;
        }
    }
}