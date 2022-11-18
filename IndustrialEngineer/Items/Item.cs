using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Item
    {
        public Item(int id, string name, Sprite sprite, int maxStackCount)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            MaxStackCount = maxStackCount;
        }

        public int Id { get; private set; }
        public int MaxStackCount { get; set; }
        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
    }
}