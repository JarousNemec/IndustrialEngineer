using IndustrialEnginner.Blocks;
using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Log:Item,IItem
    {
        public static Log Setup(Item preset)
        {
            return new Log(preset.Id, preset.Name, preset.Sprite);
        }
        
        public Log(int id, string name, Sprite sprite) : base(id, name, sprite)
        {
        }

        public Item Copy()
        {
            return new Item(Id, Name, Sprite);
        }
    }
}