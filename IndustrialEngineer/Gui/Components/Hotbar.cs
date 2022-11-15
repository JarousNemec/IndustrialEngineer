using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Hotbar : ItemStorage
    {
        public Hotbar(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows, int columns) :
            base(sprite, itemSlotSprite, itemSlotSelectedSprite, rows, columns)
        {
        }
    }
}