using System.Security.Permissions;
using IndustrialEngineer.Enums;
using IndustrialEnginner.Components;
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
        private ItemRegistry _registry;
        private ItemTransportPacket _packet;
        private Cursor _cursor;

        public GuiController(GameData gameData, View view, ItemRegistry itemRegistry, RenderWindow window, Zoom zoom,
            Cursor cursor)
        {
            _cursor = cursor;
            _packet = new ItemTransportPacket();
            _registry = itemRegistry;
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
            _packet.SourceSlotPos = GetSlotInStorage(_packet.SourceComponent, mousePosition);

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
            _packet.DestinationSlotPos = GetSlotInStorage(_packet.DestinationComponent, mousePosition);


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
                        .Id != _packet.DestinationComponent
                        .Storage[_packet.DestinationSlotPos.X, _packet.DestinationSlotPos.Y].StorageItem.Item.Id)
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

            if (addingSlot.StorageItem.Item.Id == _packet.StorageItem.Item.Id)
            {
                if (addingSlot.StorageItem.Item.MaxStackCount >=
                    _packet.StorageItem.Count + addingSlot.StorageItem.Count)
                {
                    addingSlot.AddItem(_packet.StorageItem);
                    return;
                }

                int leftToAdd = _packet.StorageItem.Count + addingSlot.StorageItem.Count -
                                addingSlot.StorageItem.Item.MaxStackCount;
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

        // private int i = 0;

        public void DrawGui(RenderWindow window, Zoom zoom)
        {
            // i++;
            // if (i > 30)
            // {
            //     i = 0;
            //     _gui.Inventory.AddItem(new StorageItem() { Item = _registry.Log.Copy(), Count = 20});
            // }

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

            return null;
        }

        public Vector2i GetSlotInStorage(ItemStorage component, Vector2i mousePosition)
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