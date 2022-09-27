using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Item
    {
        public Item(int id, string name, Sprite sprite)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
    }
}