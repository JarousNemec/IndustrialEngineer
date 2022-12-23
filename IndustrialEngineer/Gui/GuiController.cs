using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using IndustrialEngineer.Enums;
using IndustrialEnginner.Components;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner.Gui
{
    public class GuiController
    {
        private Gui _gui;

        private GuiState _state;

        // private ItemRegistry _registry;
        private ItemTransportPacket _packet;
        private Cursor _cursor;
        private GameData _gameData;

        public GuiController(GameData gameData, View view, RenderWindow window, Zoom zoom,
            Cursor cursor)
        {
            _gameData = gameData;
            _cursor = cursor;
            _packet = new ItemTransportPacket();
            // _registry = gameData.ItemRegistry;
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData, window, zoom);
            //
            // _gui.Inventory.Storage[0, 0].AddItem(new StorageItem() { Item = itemRegistry.Log.Copy() });
            // _gui.Hotbar.Storage[0, 0].AddItem(new StorageItem() { Item = itemRegistry.Log.Copy() });

            window.MouseButtonPressed += OnMousePressed;
            window.MouseButtonReleased += OnMouseReleased;
        }

        private void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    if (_state == GuiState.OpenPlayerInventory)
                    {
                        DropItem(new Vector2i(e.X, e.Y));
                    }

                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    if (_state == GuiState.OpenPlayerInventory)
                    {
                        DropItem(new Vector2i(e.X, e.Y));
                    }

                    break;
            }
        }

        private void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    if (_state == GuiState.OpenPlayerInventory)
                    {
                        DragItems(new Vector2i(e.X, e.Y));
                        ClickOnButton(new Vector2i(e.X, e.Y));
                    }

                    if (_state == GuiState.GamePlay)
                    {
                        SetItemSlotAsActiveSourceForBuilding(new Vector2i(e.X, e.Y));
                    }

                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    if (_state == GuiState.OpenPlayerInventory)
                    {
                        DragHalfItems(new Vector2i(e.X, e.Y));
                    }

                    break;
            }
        }

        private void SetItemSlotAsActiveSourceForBuilding(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if (triggeredComponent == null)
                return;
            if (triggeredComponent.Type != ComponentType.Storage &&
                triggeredComponent.Type != ComponentType.StorageSlot)
            {
                return;
            }

            Hotbar hotbar = (Hotbar)triggeredComponent;
            var selectedSlotPosition = GetSlotInGrid((ItemStorage)triggeredComponent, mousePosition);
            if (selectedSlotPosition == null)
            {
                return;
            }

            var itemSlot = hotbar.Storage[selectedSlotPosition.Value.X, selectedSlotPosition.Value.Y];


            if (hotbar.SelectedItemSlot != null && hotbar.SelectedItemSlot != itemSlot)
            {
                hotbar.SelectedItemSlot.IsSelected = false;
            }

            hotbar.SelectedItemSlot = itemSlot;
            if (itemSlot.StorageItem != null)
            {
                if (itemSlot.StorageItem.Item.Properties.Placeable)
                {
                    hotbar.SelectedItemSlot.IsSelected = !hotbar.SelectedItemSlot.IsSelected;
                }
            }
        }

        private void ClickOnButton(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if (triggeredComponent == null)
                return;
            if (triggeredComponent.Type != ComponentType.Crafting &&
                triggeredComponent.Type != ComponentType.CraftingButton)
            {
                return;
            }

            var crafting = (ClickableComponent)triggeredComponent;
            var buttonSlotPosition = GetSlotInGrid(crafting, mousePosition);
            if (buttonSlotPosition == null) return;
            Craft(_gui.Crafting.CraftingButtons[buttonSlotPosition.Value.X, buttonSlotPosition.Value.Y].Recipe);
        }

        private void Craft(Recipe recipe)
        {
            List<ItemSlot>[] restSources = new List<ItemSlot>[recipe.Ingredients.Length];
            for (int i = 0; i < restSources.Length; i++)
            {
                restSources[i] = new List<ItemSlot>();
            }

            for (int i = 0; i < recipe.Ingredients.Length; i++)
            {
                IsStorageContainingIngredient(_gui.Inventory.Storage, recipe.Ingredients[i].Id,
                    recipe.Ingredients[i].Count,
                    out List<ItemSlot> restInStorage);
                restSources[i].AddRange(restInStorage);
            }
            for (int i = 0; i < recipe.Ingredients.Length; i++)
            {
                IsStorageContainingIngredient(_gui.Hotbar.Storage, recipe.Ingredients[i].Id,
                    recipe.Ingredients[i].Count,
                    out List<ItemSlot> restInStorage);
                restSources[i].AddRange(restInStorage);
            }

            bool isPossibleToCraft = true;
            for (int i = 0; i < restSources.Length; i++)
            {
                bool result = ContainsRestItemsEnoughIngredients(restSources[i], recipe.Ingredients[i].Count);
                if (result == false)
                {
                    isPossibleToCraft = false;
                }
            }

            if (!isPossibleToCraft) return;

            var craftedItem = new StorageItem()
            {
                Item = _gameData.ItemRegistry.Registry.Find(x => x.Properties.Id == recipe.DropId),
                Count = recipe.DropCount
            };

            var restFromAdding = _gui.Hotbar.AddItem(craftedItem);
            if (restFromAdding != null)
            {
                _gui.Hotbar.RemoveItem(craftedItem.Item.Properties.Id, restFromAdding.Count);
                restFromAdding = _gui.Inventory.AddItem(craftedItem);
                if (restFromAdding != null)
                {
                    _gui.Inventory.RemoveItem(craftedItem.Item.Properties.Id, restFromAdding.Count);
                    return;
                }
            }

            for (int i = 0; i < recipe.Ingredients.Length; i++)
            {
                RemoveItems(recipe.Ingredients[i].Id, recipe.Ingredients[i].Count, restSources[i].ToArray());
            }
        }

        private bool ContainsRestItemsEnoughIngredients(List<ItemSlot> restItemSlots, int requiredCount)
        {
            int count = 0;
            foreach (var slot in restItemSlots)
            {
                count += slot.StorageItem.Count;
            }

            return count >= requiredCount;
        }

        private void RemoveItems(int id, int count, ItemSlot[] restItemsStorage)
        {
            for (int i = 0; i < restItemsStorage.GetLength(0); i++)
            {
                if (restItemsStorage[i].StorageItem.Count >= count)
                {
                    restItemsStorage[i].RemoveItem(count);
                    return;
                }

                count -= restItemsStorage[i].StorageItem.Count;
                restItemsStorage[i].RemoveItem(restItemsStorage[i].StorageItem.Count);
            }
        }

        private bool IsStorageContainingIngredient(ItemSlot[,] storage, int id, int requiredCount,
            out List<ItemSlot> restInStorage)
        {
            restInStorage = new List<ItemSlot>();
            int count = 0;
            foreach (var slot in storage)
            {
                if (slot.StorageItem?.Item.Properties.Id == id)
                {
                    count += slot.StorageItem.Count;
                    restInStorage.Add(slot);
                }
            }

            return count >= requiredCount;
        }

        private void DragItems(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if (triggeredComponent == null)
                return;
            if (triggeredComponent.Type != ComponentType.Storage &&
                triggeredComponent.Type != ComponentType.StorageSlot)
            {
                return;
            }

            _packet.SourceComponent = (ItemStorage)triggeredComponent;
            var sourceSlot = GetSlotInGrid(_packet.SourceComponent, mousePosition);
            if (sourceSlot == null)
            {
                return;
            }

            _packet.SourceSlotPos = (Vector2i)sourceSlot;
            var storageItem = _packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y]
                .StorageItem;
            if (storageItem == null)
                return;
            _packet.StorageItem = new StorageItem() { Item = storageItem.Item, Count = storageItem.Count };
            _cursor.SetActiveItemIcon(_packet.StorageItem.Item);
        }

        private void DragHalfItems(Vector2i mousePosition)
        {
            DragItems(mousePosition);
            _packet.DragHalf = true;
        }

        private void DropItem(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if (triggeredComponent == null)
            {
                if (_packet.StorageItem != null)
                    _cursor.RemoveActiveItemIcon();
                return;
            }

            if (triggeredComponent.Type != ComponentType.Storage &&
                triggeredComponent.Type != ComponentType.StorageSlot)
            {
                if (_packet.StorageItem != null)
                    _cursor.RemoveActiveItemIcon();
                return;
            }

            if (_packet.StorageItem == null)
                return;
            _packet.DestinationComponent = (ItemStorage)triggeredComponent;
            var destinationSlot = GetSlotInGrid(_packet.DestinationComponent, mousePosition);
            if (destinationSlot == null)
                return;
            _packet.DestinationSlotPos = (Vector2i)destinationSlot;
            if (!_packet.DragHalf)
            {
                Drop();
            }
            else
            {
                if ((_packet.StorageItem.Count / 2) < 1)
                {
                    Drop();
                }
                else
                {
                    DropHalf();
                }
            }

            _cursor.RemoveActiveItemIcon();
        }

        private void DropHalf()
        {
            _packet.DragHalf = false;
            int count = _packet.StorageItem.Count / 2;
            if (_packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y]
                .RemoveItem(count < 1 ? 1 : count))
            {
                AddItemToDestinationSlot(count);
            }
        }

        private void Drop()
        {
            if (_packet.DestinationComponent.Storage[_packet.DestinationSlotPos.X, _packet.DestinationSlotPos.Y]
                    .StorageItem != null)
            {
                if (_packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y].StorageItem.Item
                        .Properties
                        .Id != _packet.DestinationComponent
                        .Storage[_packet.DestinationSlotPos.X, _packet.DestinationSlotPos.Y].StorageItem.Item.Properties
                        .Id)
                {
                    return;
                }
            }

            if (_packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y]
                .RemoveItem(_packet.StorageItem.Count))
            {
                AddItemToDestinationSlot(_packet.StorageItem.Count);
            }
        }

        private void AddItemToDestinationSlot(int count)
        {
            _packet.StorageItem.Count = count;
            var addingSlot =
                _packet.DestinationComponent.Storage[_packet.DestinationSlotPos.X, _packet.DestinationSlotPos.Y];
            repeat:
            if (addingSlot.StorageItem == null)
            {
                addingSlot.AddItem(_packet.StorageItem);
                return;
            }

            if (addingSlot.StorageItem.Item.Properties.Id == _packet.StorageItem.Item.Properties.Id)
            {
                if (addingSlot.StorageItem.Item.Properties.MaxStackCount >=
                    _packet.StorageItem.Count + addingSlot.StorageItem.Count)
                {
                    addingSlot.AddItem(_packet.StorageItem);
                    return;
                }

                int leftToAdd = _packet.StorageItem.Count + addingSlot.StorageItem.Count -
                                addingSlot.StorageItem.Item.Properties.MaxStackCount;
                addingSlot.AddItem(new StorageItem()
                    { Item = _packet.StorageItem.Item, Count = _packet.StorageItem.Count - leftToAdd });
                _packet.StorageItem.Count -= _packet.StorageItem.Count - leftToAdd;
                _packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y]
                    .AddItem(_packet.StorageItem);
                return;
            }

            return;
        }

        public Gui GetGui()
        {
            return _gui;
        }


        public void UpdatePosition(View view, Zoom zoom)
        {
            _gui.ActualizeComponentsPositions(view, zoom);
        }

        public void DrawGui(RenderWindow window, Zoom zoom)
        {
            _gui.DrawComponents(window, zoom, _state);
        }

        public void OpenOrClosePlayerInventory()
        {
            if (_state == GuiState.GamePlay)
            {
                _state = GuiState.OpenPlayerInventory;
            }
            else
            {
                _state = GuiState.GamePlay;
            }
        }

        public GuiState GetGuiState()
        {
            return _state;
        }

        public GuiComponent GetClickedComponent(Vector2i mousePosition)
        {
            if (_state == GuiState.OpenPlayerInventory)
            {
                if (IsPointInArea(mousePosition, _gui.Hotbar.ClickGrid.ClickArea.LeftUpCorner,
                        _gui.Hotbar.ClickGrid.ClickArea.RightDownCorner))
                {
                    return _gui.Hotbar;
                }

                if (IsPointInArea(mousePosition, _gui.Inventory.ClickGrid.ClickArea.LeftUpCorner,
                        _gui.Inventory.ClickGrid.ClickArea.RightDownCorner))
                {
                    return _gui.Inventory;
                }

                if (IsPointInArea(mousePosition, _gui.Crafting.ClickGrid.ClickArea.LeftUpCorner,
                        _gui.Crafting.ClickGrid.ClickArea.RightDownCorner))
                {
                    return _gui.Crafting;
                }
            }

            if (_state == GuiState.GamePlay)
            {
                if (IsPointInArea(mousePosition, _gui.Hotbar.ClickGrid.ClickArea.LeftUpCorner,
                        _gui.Hotbar.ClickGrid.ClickArea.RightDownCorner))
                {
                    return _gui.Hotbar;
                }
            }


            return null;
        }

        public Vector2i? GetSlotInGrid(ClickableComponent component, Vector2i mousePosition)
        {
            return component.ClickGrid.GetCurrentCell(mousePosition);
        }

        private bool IsPointInArea(Vector2i point, Vector2i leftUpCorner, Vector2i rightDownCorner)
        {
            return leftUpCorner.X < point.X && rightDownCorner.X > point.X && leftUpCorner.Y < point.Y &&
                   rightDownCorner.Y > point.Y;
        }
    }
}