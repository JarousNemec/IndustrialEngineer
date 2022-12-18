using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class ItemProperties
    {
        public int Id { get; private set; }
        public int MaxStackCount { get; set; }
        public string Name { get; private set; }
        public Sprite Sprite { get; private set; }
        
        public bool Placeable { get; set; }
        public int PlacedEntityId { get; set; }
        
        public ItemProperties(int id, string name, Sprite sprite, int maxStackCount, bool placeable = false, int placedEntityId = 0)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            MaxStackCount = maxStackCount;
            Placeable = placeable;
            PlacedEntityId = placedEntityId;
        }

        public ItemProperties Copy()
        {
            return new ItemProperties(Id, Name, Sprite, MaxStackCount, Placeable, PlacedEntityId);
        }
    }
}