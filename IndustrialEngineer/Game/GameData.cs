using System.Collections.Generic;
using System.IO;
using IndustrialEngineer.Blocks;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Gui;
using IndustrialEnginner.Items;
using SFML.Graphics;

namespace IndustrialEnginner
{
    public static class GameData
    {
        public static BlockRegistry BlockRegistry { get; set; }
        public static ItemRegistry ItemRegistry { get; set; }
        public static BuildingsRegistry BuildingsRegistry { get; set; }

        public static DialogsRegistry DialogsRegistry { get; set; }
        public static RecipesRegistry RecipesRegistry { get; set; }
        public const string TEXTURES_DIRECTORY_PATH = "./assest/textures/";
        public static Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        public const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";
        public static Font Font;
    }
}