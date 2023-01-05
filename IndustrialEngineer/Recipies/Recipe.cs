using System.Collections.Generic;
using System.Linq;
using IndustrialEngineer.Enums;
using IndustrialEnginner.GameEntities;
using SFML.Graphics;

namespace IndustrialEnginner.CraftingRecipies
{
    public class Recipe
    {
        public string Name { get; set; }
        public Sprite Sprite { get; set; }
        public int Id { get; set; }
        public int DropId { get; set; }
        public int DropCount { get; set; }
        public RecipeType RecipeType { get; set; }
        public Ingredient[] Ingredients { get; set; }
        public List<Ingredient> IngredientsList { get; set; }

        public Recipe(string name, Sprite sprite, int id, int dropId, int dropCount, RecipeType recipeType, Ingredient[] ingredients)
        {
            Name = name;
            Sprite = sprite;
            Id = id;
            DropId = dropId;
            DropCount = dropCount;
            RecipeType = recipeType;
            Ingredients = ingredients;
            IngredientsList = Ingredients.ToList();
        }
    }
}