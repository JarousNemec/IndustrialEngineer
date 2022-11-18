using IndustrialEnginner.Blocks;
using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Log:Item,IItem
    {
        public static Log Setup(Item preset)
        {
            return new Log(preset.Id, preset.Name, preset.Sprite, preset.MaxStackCount);
        }
        
        public Log(int id, string name, Sprite sprite, int maxStackCount) : base(id, name, sprite, maxStackCount)
        {
        }

        public Item Copy()
        {
            return new Item(Id, Name, Sprite, MaxStackCount);
        }
    }
}