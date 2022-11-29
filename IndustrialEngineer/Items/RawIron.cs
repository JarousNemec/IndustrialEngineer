using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class RawIron:Item,IItem
    {
        public static RawIron Setup(Item preset)
        {
            return new RawIron(preset.Id, preset.Name, preset.Sprite, preset.MaxStackCount);
        }
        
        public RawIron(int id, string name, Sprite sprite, int maxStackCount) : base(id, name, sprite, maxStackCount)
        {
        }

        public Item Copy()
        {
            return new Item(Id, Name, Sprite, MaxStackCount);
        }
    }
}