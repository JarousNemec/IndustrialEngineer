using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Item
    {
        public ItemProperties Properties { get; set; }
        public Item(ItemProperties properties)
        {
            Properties = properties;
        }

        public Item Copy()
        {
            return new Item(Properties.Copy());
        }
        
    }
}