using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Stone:Item,IItem
    {
        public static Stone Setup(Item preset)
        {
            return new Stone(preset.Id, preset.Name, preset.Sprite, preset.MaxStackCount);
        }
        
        public Stone(int id, string name, Sprite sprite, int maxStackCount) : base(id, name, sprite, maxStackCount)
        {
        }

        public Item Copy()
        {
            return new Item(Id, Name, Sprite, MaxStackCount);
        }
    }
}