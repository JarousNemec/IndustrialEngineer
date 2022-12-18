using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class IronIngot:Item,IItem
    {
        public IronIngot(ItemProperties properties) : base(properties)
        {
        }
    }
}