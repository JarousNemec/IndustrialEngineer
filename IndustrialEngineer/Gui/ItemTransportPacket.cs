using System.Security.AccessControl;
using System.Security.Permissions;
using IndustrialEnginner.Items;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class ItemTransportPacket
    {
        public Vector2i SourceSlotPos { get; set; }
        public ItemStorage SourceComponent { get; set; }
        
        public Vector2i DestinationSlotPos { get; set; }
        public ItemStorage DestinationComponent { get; set; }
        public StorageItem StorageItem { get; set; }
    }
}