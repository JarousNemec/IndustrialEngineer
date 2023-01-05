using IndustrialEnginner.Gui;

namespace IndustrialEnginner.GameEntities
{
    public class Drill : Building
    {
        public ItemSlot InputFuelSlot { get; set; }
        public ItemStorage OutputStorage { get; set; }

        public Drill(BuildingProperties properties) : base(properties)
        {
            _mining = new Mining();
        }

        public override void Update(float deltaTime, World world)
        {
            CheckAndConsumeFuel();
            Mine(deltaTime, world);
        }

        private Mining _mining;


        private void CheckAndConsumeFuel()
        {
            if (EnergyBuffer >= GameData.ItemRegistry.Energy.Properties.MaxStackCount) return;
            if (InputFuelSlot.StorageItem == null) return;
            if (InputFuelSlot.StorageItem.Count <= 0) return;
            if (!InputFuelSlot.StorageItem.Item.Properties.Flammable) return;
            if (InputFuelSlot.StorageItem.Item.Properties.CalorificValue +
                EnergyBuffer >= Properties.MaximalBufferedEnergyValue) return;
            var item = InputFuelSlot.StorageItem.Item;
            if (InputFuelSlot.RemoveItem(1))
            {
                EnergyBuffer += item.Properties.CalorificValue;
                DebugUtil.DebugTexts[6].DisplayedString = EnergyBuffer.ToString();
            }
        }

        public int EnergyBuffer { get; set; } = 0;
        public int EnergyConsumePerOneMine { get; set; } = 10;

        private void Mine(float deltaTime, World world)
        {
            if (!_mining.IsMining)
            {
                SetupMining();
                return;
            }

            _mining.ActualProgress += _mining.Speed * deltaTime;
            if (_mining.ActualProgress < _mining.FinishValue)
            {
                Properties.Dialog.ProgressBarComponent.Sprite = Properties.Dialog.ProgressBarComponent.States[
                    ProgressBarComponent.CalculateCurrentStateIndex(
                        Properties.Dialog.ProgressBarComponent.States.Length, _mining.FinishValue,
                        _mining.ActualProgress)];
                return;
            }

            _mining.IsMining = false;
            Properties.Sprite = Properties.States[0];
            Properties.Dialog.Sprite = Properties.Dialog.States[0];
            Properties.Dialog.ProgressBarComponent.Sprite = Properties.Dialog.ProgressBarComponent.States[0];

            var selectedBlock = world.Map[(int)Properties.X, (int)Properties.Y];
            StorageItem itemToAdd = new StorageItem()
            {
                Item =
                    GameData.ItemRegistry.Registry.Find(x =>
                        x.Properties.Id == selectedBlock.Properties.DropId),
                Count = selectedBlock.Properties.DropCount
            };
            StorageItem returnedStorageItem = OutputStorage.AddItem(itemToAdd);

            if (returnedStorageItem == null)
            {
                EnergyBuffer -= EnergyConsumePerOneMine;
                DebugUtil.DebugTexts[6].DisplayedString = EnergyBuffer.ToString();
                selectedBlock.Properties.Richness--;
                selectedBlock.Properties.DropCount =
                    selectedBlock.Properties.OriginalDropCount;
                if (selectedBlock.Properties.Richness <= 0)
                {
                    var entity = selectedBlock.Properties.PlacedBuilding;
                    world.Map[(int)Properties.X, (int)Properties.Y] = GameData.BlockRegistry.Registry.Find(x =>
                        x.Properties.Id ==
                        selectedBlock.Properties.FoundationId);
                    world.Map[(int)Properties.X, (int)Properties.Y].PlaceEntity(entity);
                    entity.Properties.FoundationBlock = world.Map[(int)Properties.X, (int)Properties.Y];
                    world.Manager.UpdateMap();
                }
            }
            else
            {
                selectedBlock.Properties.DropCount =
                    returnedStorageItem.Count;
            }
        }

        private void SetupMining()
        {
            if (Properties.FoundationBlock.Properties.Harvestable &&
                Properties.FoundationBlock.Properties.Richness > 0 && EnergyBuffer >= EnergyConsumePerOneMine)
            {
                _mining.IsMining = true;
                _mining.FinishValue = Properties.FoundationBlock.Properties.HarvestTime;
                _mining.ActualProgress = 0.01f;
                Properties.Sprite = Properties.States[1];
                Properties.Dialog.Sprite = Properties.Dialog.States[1];
            }
        }
    }
}