using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemStorage : ClickableComponent
    {
        public ItemSlot[,] Storage { get; set; }
        public Vector2u _itemSlotSize;
        private GameData _gameData;

        public ItemStorage(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows,
            int columns, GameData gameData) : base(sprite, ComponentType.Storage, rows, columns)
        {
            _gameData = gameData;
            _itemSlotSize = itemSlotSprite.Texture.Size;
            Storage = new ItemSlot[columns, rows];
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    var slot = new ItemSlot(itemSlotSprite, itemSlotSelectedSprite, gameData,
                        ComponentType.StorageSlot);
                    Storage[i, j] = slot;
                    _childComponentsToDraw.Add(slot);
                }
            }
        }
        
        public ItemStorage(Sprite sprite,Sprite itemSlotSprite, GameData gameData, int rows, int columns) : base(sprite, ComponentType.Storage, rows, columns)
        {
            _gameData = gameData;
            _itemSlotSize = itemSlotSprite.Texture.Size;
        }

        public ItemStorage Copy()
        {
            var newStorage = new ItemStorage(Sprite,Storage[0,0].Sprite,_gameData, Storage.GetLength(1),
                Storage.GetLength(0))
            {
                ClickGrid = ClickGrid
            };
            newStorage.Storage = new ItemSlot[Storage.GetLength(0), Storage.GetLength(1)];
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    var slot = Storage[i, j].Copy();
                    newStorage.Storage[i, j] = slot;
                    newStorage._childComponentsToDraw.Add(slot);
                }
            }

            return newStorage;
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

            canMaxAdd = slotEnableToAddItem.StorageItem.Item.Properties.MaxStackCount -
                        slotEnableToAddItem.StorageItem.Count;
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

        public void RemoveItem(int id, int count)
        {
            for (int i = 0; i < Storage.GetLength(0); i++)
            {
                for (int j = 0; j < Storage.GetLength(1); j++)
                {
                    if (Storage[i, j].StorageItem?.Item.Properties.Id != id) continue;
                    if (Storage[i, j].StorageItem.Count >= count)
                    {
                        Storage[i, j].RemoveItem(count);
                        return;
                    }

                    count -= Storage[i, j].StorageItem.Count;
                    Storage[i, j].RemoveItem(Storage[i, j].StorageItem.Count);
                }
            }
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
                    if (storageItem.Item.Properties.Id == item.Properties.Id &&
                        storageItem.Count < storageItem.Item.Properties.MaxStackCount)
                        return storage[y, x];
                }
            }

            return null;
        }
    }
}