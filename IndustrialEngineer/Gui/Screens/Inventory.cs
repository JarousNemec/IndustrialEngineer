using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Inventory : ItemStorage
    {
        public Inventory(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows, int columns) :
            base(sprite, itemSlotSprite, itemSlotSelectedSprite, rows, columns)
        {
        }
    }
}