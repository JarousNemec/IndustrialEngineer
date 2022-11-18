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

        public GuiController(GameData gameData, View view, ItemRegistry itemRegistry, RenderWindow window, Zoom zoom, Cursor cursor)
        {
            _cursor = cursor;
            _packet = new ItemTransportPacket();
            _registry = itemRegistry;
            _state = GuiState.GamePlay;
            _gui = new Gui(gameData, window, zoom);
            
            _gui.Inventory.Storage[0, 0].AddItem(new StorageItem(){Item = itemRegistry.Log.Copy()});
            _gui.Hotbar.Storage[0,0].AddItem(new StorageItem(){Item = itemRegistry.Log.Copy()});
            
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
                        DragItem(new Vector2i(e.X, e.Y));
                    }
                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    break;
            }
        }

        public void DragItem(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if(triggeredComponent == null)
                return;
            if (triggeredComponent.Type != ComponentType.Storage &&
                triggeredComponent.Type != ComponentType.StorageSlot)
            {
                return;
            }
            _packet.SourceComponent = (ItemStorage)triggeredComponent;
            _packet.SourceSlotPos = GetSlotInStorage(_packet.SourceComponent, mousePosition);
            _packet.StorageItem = _packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y]
                .StorageItem;
            if(_packet.StorageItem==null)
                return;
            _cursor.SetActiveItem(_packet.StorageItem.Item);
        }

        public void DropItem(Vector2i mousePosition)
        {
            var triggeredComponent = GetClickedComponent(mousePosition);
            if(triggeredComponent == null)
                return;
            if (triggeredComponent.Type != ComponentType.Storage &&
                triggeredComponent.Type != ComponentType.StorageSlot)
            {
                return;
            }
            if(_packet.StorageItem==null)
                return;
            _packet.DestinationComponent = (ItemStorage)triggeredComponent;
            _packet.DestinationSlotPos = GetSlotInStorage(_packet.DestinationComponent, mousePosition);
            _packet.SourceComponent.Storage[_packet.SourceSlotPos.X, _packet.SourceSlotPos.Y].RemoveItem();
            _packet.DestinationComponent.Storage[_packet.DestinationSlotPos.X, _packet.DestinationSlotPos.Y]
                .AddItem(_packet.StorageItem);
            _cursor.RemoveActiveItem();
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
            //     if (i > 1)
            //     {
            //         i = 0;
            //         _gui.Inventory.Storage[0, 0].AddItem(_registry.Log.Copy());
            //     }
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
                var cell = _gui.Hotbar.ClickGrid.GetCurrentCell(mousePosition);
                var packet = EncapsuleSourceOfItemTransportPacket(cell, _gui.Hotbar);
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

        private ItemTransportPacket EncapsuleSourceOfItemTransportPacket(Vector2i cell, GuiComponent guiComponent)
        {
            ItemTransportPacket packet = new ItemTransportPacket();
            // packet.SourceComponent = guiComponent;
            packet.SourceSlotPos = cell;
            return packet;
        }

        private bool IsPointInArea(Vector2i point, Vector2i leftUpCorner, Vector2i rightDownCorner)
        {
            return leftUpCorner.X < point.X && rightDownCorner.X > point.X && leftUpCorner.Y < point.Y &&
                   rightDownCorner.Y > point.Y;
        }
    }
}