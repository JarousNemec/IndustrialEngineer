using System.Collections.Generic;
using System.IO;
using SFML.Graphics;

namespace IndustrialEnginner
{
    public class GameData
    {
        private const string TEXTURES_DIRECTORY_PATH = "./textures/";
        private Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        public GameData()
        {
            Sprites = SpriteFactory.LoadSprites(TEXTURES_DIRECTORY_PATH);
        }
        

        public Dictionary<string, Sprite> GetSprites()
        {
            return Sprites;
        }
    }
}