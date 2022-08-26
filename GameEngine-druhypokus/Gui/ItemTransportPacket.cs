using IndustrialEnginner.Items;

namespace IndustrialEnginner.Gui
{
    public class ItemTransportPacket
    {
        public int Count { get; set; }
        public Item Item { get; set; }

        public ItemTransportPacket(int count,Item item)
        {
            Count = count;
            Item = item;
        }
    }
}