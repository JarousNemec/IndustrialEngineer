using IndustrialEnginner.Blocks;
using IndustrialEnginner.Interfaces;
using SFML.Graphics;

namespace IndustrialEnginner.Items
{
    public class Log : Item, IItem
    {
        public Log(ItemProperties properties) : base(properties)
        {
        }
    }
}