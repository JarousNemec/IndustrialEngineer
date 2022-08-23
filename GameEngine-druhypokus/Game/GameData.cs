using System.Collections.Generic;
using System.IO;
using SFML.Graphics;

namespace IndustrialEnginner
{
    public class GameData
    {
        private const string TEXTURES_DIRECTORY_PATH = "./textures/";
        private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
        private const int SPRITES_SIZE = 20;

        public GameData()
        {
            Sprites = SpriteFactory.LoadSprites(TEXTURES_DIRECTORY_PATH,SPRITES_SIZE);
        }
        

        public Dictionary<string, Sprite> GetSprites()
        {
            return Sprites;
        }
    }
}