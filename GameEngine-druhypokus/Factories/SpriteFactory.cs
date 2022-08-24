using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner
{
    public class SpriteFactory
    {
        public static Dictionary<string, Sprite> LoadSprites(string TEXTURES_DIRECTORY_PATH)
        {
            Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();
            string[] texturesPaths = Directory.GetFiles(TEXTURES_DIRECTORY_PATH);

            foreach (var texturePath in texturesPaths)
            {
                Texture texture = new Texture(texturePath);
                texture.Smooth = false;
                Sprite sprite = new Sprite(texture);
                
                Sprites.Add(Path.GetFileNameWithoutExtension(texturePath), sprite);
            }

            return Sprites;
        }
    }
}