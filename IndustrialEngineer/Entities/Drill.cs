using IndustrialEnginner.Gui;
using IndustrialEnginner.Items;
using SFML.System;

namespace IndustrialEnginner.GameEntities
{
    public class Drill : Building
    {
        public Drill(BuildingProperties properties) : base(properties)
        {
            _mining = new Mining();
        }

        public override void Update(float deltaTime, World world)
        {
            base.Update(deltaTime,world);
            // CheckAndConsumeFuel();
            // Mine(deltaTime, world);
        }

        private Mining _mining;
        private bool _miningInProgress = false;

        private void CheckAndConsumeFuel()
        {
            
        }
        public int EnergyBuffer { get; set; } = 0;
        private void Mine(float deltaTime, World world)
        {
            if (!_miningInProgress)
            {
                SetupMining();
                return;
            }
            
            _mining.ActualProgress += _mining.speed * deltaTime;
            if (_mining.ActualProgress < _mining.FinishValue)
            {
                return;
            }

            _mining.IsMining = false;

            // if (selectedBlock.Properties.PlacedEntity == null)
            // {
            //     StorageItem itemToAdd = new StorageItem()
            //     {
            //         Item =
            //             GameData.ItemRegistry.Registry.Find(x =>
            //                 x.Properties.Id == selectedBlock.Properties.DropId),
            //         Count = selectedBlock.Properties.DropCount
            //     };
            //     StorageItem returnedStorageItem = _player.Inventory.AddItem(itemToAdd);
            //     if (returnedStorageItem != null)
            //         returnedStorageItem = _player.Hotbar.AddItem(returnedStorageItem);
            //     if (returnedStorageItem == null)
            //     {
            //         selectedBlock.Properties.Richness--;
            //         selectedBlock.Properties.DropCount =
            //             selectedBlock.Properties.OriginalDropCount;
            //         if (selectedBlock.Properties.Richness <= 0)
            //         {
            //             _world.Map[_cursorWorldPos.X, _cursorWorldPos.Y] = GameData.BlockRegistry.Registry.Find(x =>
            //                 x.Properties.Id ==
            //                 selectedBlock.Properties.FoundationId);
            //             _worldManager.UpdateMap();
            //         }
            //     }
            //     else
            //     {
            //         selectedBlock.Properties.DropCount =
            //             returnedStorageItem.Count;
            //     }
            // }
            // else
            // {
            //     StorageItem itemToAdd = new StorageItem()
            //     {
            //         Item =
            //             GameData.ItemRegistry.Registry.Find(x =>
            //                 x.Properties.Id == selectedBlock.Properties.PlacedEntity.Properties.DropItemId),
            //         Count = 1
            //     };
            //     StorageItem returnedStorageItem = _player.Hotbar.AddItem(itemToAdd);
            //     if (returnedStorageItem == null)
            //     {
            //         selectedBlock.RemoveEntity();
            //         _worldManager.UpdateMap();
            //     }
            //     else
            //     {
            //         returnedStorageItem = _player.Inventory.AddItem(itemToAdd);
            //         if (returnedStorageItem == null)
            //         {
            //             selectedBlock.RemoveEntity();
            //             _worldManager.UpdateMap();
            //         }
            //     }
            // }
        }

        private void SetupMining()
        {
            if (Properties.FoundationBlock.Properties.Harvestable && EnergyBuffer > 10)
            {
                _mining.IsMining = true;
                _mining.FinishValue = Properties.FoundationBlock.Properties.HarvestTime;
                _mining.ActualProgress = 0.01f;
            }
        }
    }
}