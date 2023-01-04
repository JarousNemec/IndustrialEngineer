using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using GameEngine_druhypokus.Factories;
using IndustrialEngineer.Enums;
using IndustrialEnginner;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.Items;
using SFML.Graphics;

namespace IndustrialEngineer.Factories
{
    public class RecipeFactory
    {
        public static RecipesRegistry LoadRecipes(string path)
        {
            RecipesRegistry registry = new RecipesRegistry();
            var presets = LoadJson(path);
            var recipes = RecipesRegistrySetup(presets);

            registry.CraftingRecipes = recipes.FindAll(x => x.RecipeType == RecipeType.Crafting);
            registry.SmeltingRecipes = recipes.FindAll(x => x.RecipeType == RecipeType.Smelting);
            return registry;
        }

        private static List<RecipePreset> LoadJson(string path)
        {
            List<RecipePreset> presets;
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                presets = JsonSerializer.Deserialize<List<RecipePreset>>(json);
            }

            return presets;
        }

        private static List<Recipe> RecipesRegistrySetup(List<RecipePreset> presets)
        {
            List<Recipe> recipes = new List<Recipe>();
            foreach (var preset in presets)
            {
                recipes.Add(new Recipe(preset.Name, GameData.Sprites[preset.Texture], preset.Id, preset.DropId,
                    preset.DropCount, (RecipeType)preset.RecipeType, preset.Ingredients));
            }

            return recipes;
        }
    }

    public class RecipePreset
    {
        public string Name { get; set; }
        public string Texture { get; set; }
        public int Id { get; set; }
        public int DropId { get; set; }
        public int DropCount { get; set; }
        public int RecipeType { get; set; }
        public Ingredient[] Ingredients { get; set; }
    }
}