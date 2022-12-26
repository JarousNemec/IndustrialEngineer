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
    public class GameData
    {
        public BlockRegistry BlockRegistry { get; set; }
        public ItemRegistry ItemRegistry { get; set; }
        public BuildingsRegistry BuildingsRegistry { get; set; }

        public DialogsRegistry DialogsRegistry { get; set; }
        public RecipesRegistry RecipesRegistry { get; set; }
        private const string TEXTURES_DIRECTORY_PATH = "./assest/textures/";
        private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        // public const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";font.psp
        public const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";
        public static Font Font;
        public GameData()
        {
            Font = new Font(CONSOLE_FONT_PATH);
            Sprites = SpriteFactory.LoadSprites(TEXTURES_DIRECTORY_PATH);
        }
        

        public Sprite GetSprite(string key)
        {
            return Sprites[key];
        }
    }
}