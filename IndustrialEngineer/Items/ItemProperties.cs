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
        public bool Flammable { get; set; }
        public int CalorificValue { get; set; }
        
        public ItemProperties(int id, string name, Sprite sprite, int maxStackCount,bool flammable, int calorificValue, bool placeable = false, int placedEntityId = 0)
        {
            Id = id;
            Name = name;
            Sprite = sprite;
            MaxStackCount = maxStackCount;
            Placeable = placeable;
            PlacedEntityId = placedEntityId;
            Flammable = flammable;
            CalorificValue = calorificValue;
        }

        public ItemProperties Copy()
        {
            return new ItemProperties(Id, Name, Sprite, MaxStackCount,Flammable, CalorificValue, Placeable, PlacedEntityId);
        }
    }
}