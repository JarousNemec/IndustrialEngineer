using System.Runtime.InteropServices;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class Hotbar : ItemStorage
    {
        public ItemSlot SelectedItemSlot { get; set; }
        public Hotbar(Sprite sprite, Sprite itemSlotSprite, Sprite itemSlotSelectedSprite, int rows, int columns, GameData gameData) :
            base(sprite, itemSlotSprite, itemSlotSelectedSprite, rows, columns, gameData)
        {
        }
    }
}