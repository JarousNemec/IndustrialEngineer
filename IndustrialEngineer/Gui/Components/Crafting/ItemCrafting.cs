using IndustrialEngineer.Enums;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemCrafting : ClickableComponent
    {
        public CraftingButton[,] CraftingButtons { get; set; }
        private Vector2u _craftingButtonSize;
        public ItemCrafting(Sprite sprite, Sprite craftingButtonSprite, RecipesRegistry registry, ComponentType type,int rows, int columns) : base(sprite, type, rows, columns)
        {
            _craftingButtonSize = craftingButtonSprite.Texture.Size;

            CraftingButtons = new CraftingButton[columns, rows];

            int recipeIndex = registry.CraftingRecipes.Count-1;

            for (int i = 0; i < CraftingButtons.GetLength(0); i++)
            {
                for (int j = 0; j < CraftingButtons.GetLength(1); j++)
                {
                    if (recipeIndex >= 0)
                    {
                        var button = new CraftingButton(craftingButtonSprite, registry.CraftingRecipes[recipeIndex],
                            ComponentType.CraftingButton);
                        button.GridPositionX = j;
                        button.GridPositionY = i;
                        CraftingButtons[i, j] = button;
                        _childComponentsToDraw.Add(button);
                        recipeIndex--;
                    }
                }
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom, float marginX = 0,
            float marginY = 0, float marginBetween = 0)
        {
            base.ActualizeDisplayingCords(newX, newY);
            for (int i = 0; i < _childComponentsToDraw.Count; i++)
            {
                var component = (CraftingButton)_childComponentsToDraw[i];
                component.ActualizeDisplayingCords((zoom.FlippedZoomed * component.GridPositionY * _craftingButtonSize.X) + newX + marginX * zoom.FlippedZoomed + component.GridPositionY * marginBetween * zoom.FlippedZoomed, (zoom.FlippedZoomed * component.GridPositionX * _craftingButtonSize.Y) + newY + marginY * zoom.FlippedZoomed +
                    component.GridPositionX * marginBetween * zoom.FlippedZoomed, zoom, _craftingButtonSize);

            }
        }
    }
}