using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Stone:Item,IItem
    {
        public Stone(ItemProperties properties) : base(properties)
        {
        }
    }
}