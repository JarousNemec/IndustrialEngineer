using System.Linq;
using IndustrialEngineer.Enums;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.Gui;

namespace IndustrialEnginner.GameEntities
{
    public class Furnace : Building
    {
        private Crafting _crafting;
        public ItemSlot InputFuelSlot { get; set; }
        public ItemSlot InputIngredientSlot { get; set; }
        public ItemStorage OutputStorage { get; set; }
        public int EnergyBuffer { get; set; } = 0;
        public int EnergyConsumePerOneSmelt { get; set; } = 10;

        private Recipe _actualRecipe;
        private Ingredient _mainIngredient;

        public Furnace(BuildingProperties properties) : base(properties)
        {
            _crafting = new Crafting();
        }

        public override void Update(float deltaTime, World world)
        {
            CheckAndConsumeFuel();
            Smelt(deltaTime, world);
        }

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
                DebugUtil.DebugTexts[7].DisplayedString = EnergyBuffer.ToString();
            }
        }

        private void Smelt(float deltaTime, World world)
        {
            if (!_crafting.IsCrafting)
            {
                SetupCrafting();
                return;
            }

            _crafting.ActualProgress += _crafting.Speed * deltaTime;
            if (_crafting.FinishValue > _crafting.ActualProgress)
            {
                Properties.Dialog.ProgressBarComponent.Sprite = Properties.Dialog.ProgressBarComponent.States[
                    ProgressBarComponent.CalculateCurrentStateIndex(
                        Properties.Dialog.ProgressBarComponent.States.Length, _crafting.FinishValue,
                        _crafting.ActualProgress)];
                return;
            }


            _crafting.IsCrafting = false;
            Properties.Sprite = Properties.States[0];
            Properties.Dialog.Sprite = Properties.Dialog.States[0];
            Properties.Dialog.ProgressBarComponent.Sprite = Properties.Dialog.ProgressBarComponent.States[0];

            var selectedBlock = world.Map[(int)Properties.X, (int)Properties.Y];
            StorageItem itemToAdd = new StorageItem()
            {
                Item =
                    GameData.ItemRegistry.Registry.Find(x =>
                        x.Properties.Id == _actualRecipe.DropId),
                Count = _actualRecipe.DropCount
            };
            StorageItem returnedStorageItem = OutputStorage.AddItem(itemToAdd);

            if (returnedStorageItem != null) return;

            if (InputIngredientSlot.StorageItem == null) return;
            if (InputIngredientSlot.StorageItem.Item.Properties.Id != _mainIngredient.Id) return;
            if (InputIngredientSlot.RemoveItem(_mainIngredient.Count))
            {
                EnergyBuffer -= EnergyConsumePerOneSmelt;
                DebugUtil.DebugTexts[7].DisplayedString = EnergyBuffer.ToString();
                return;
            }

            OutputStorage.RemoveItem(itemToAdd.Item.Properties.Id, itemToAdd.Count);
        }

        private void SetupCrafting()
        {
            if (InputIngredientSlot.StorageItem == null) return;
            if (InputIngredientSlot.StorageItem.Count <= 0) return;

            _actualRecipe = GameData.RecipesRegistry.SmeltingRecipes.Find(x =>
                x.IngredientsList.Find(y => y.Id == InputIngredientSlot.StorageItem.Item.Properties.Id) != null);
            if (_actualRecipe == null) return;
            _mainIngredient =
                _actualRecipe.IngredientsList.Find(y => y.Id == InputIngredientSlot.StorageItem.Item.Properties.Id);
            if (_actualRecipe.RecipeType != RecipeType.Smelting) return;
            if (EnergyBuffer >= _actualRecipe.Ingredients
                    .First(x => x.Id == GameData.ItemRegistry.Energy.Properties.Id)?.Count)
            {
                _crafting.IsCrafting = true;
                _crafting.ActualProgress = 0.01f;
                _crafting.FinishValue = _actualRecipe.IngredientsList
                    .Find(x => x.Id == GameData.ItemRegistry.Time.Properties.Id).Count;
                EnergyConsumePerOneSmelt = _actualRecipe.IngredientsList
                    .Find(x => x.Id == GameData.ItemRegistry.Energy.Properties.Id).Count;
                Properties.Sprite = Properties.States[1];
                Properties.Dialog.Sprite = Properties.Dialog.States[1];
            }
        }
    }
}