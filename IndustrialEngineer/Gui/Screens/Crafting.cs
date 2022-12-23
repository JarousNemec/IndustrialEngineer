using IndustrialEngineer.Enums;
using IndustrialEnginner.CraftingRecipies;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Crafting : ItemCrafting
    {
        public Crafting(Sprite sprite, Sprite craftingButtonSprite, RecipesRegistry registry, ComponentType type,int rows, int columns) : base(sprite, craftingButtonSprite, registry, type,rows, columns)
        {
        }
    }
}