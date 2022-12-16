using IndustrialEnginner.Enums;
using IndustrialEnginner.Gui;
using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class PlaceableEntityProperties : GraphicsEntityProperties
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public BlockType CanBePlacedOnType { get; set; }
        public GuiComponent Gui { get; set; }

        public PlaceableEntityProperties(Sprite sprite, Sprite[] states, string name, int id,
            BlockType canBePlacedOnType, GuiComponent gui = null) : base(sprite, states)
        {
            Name = name;
            Id = id;
            CanBePlacedOnType = canBePlacedOnType;
            Gui = gui;
        }

        public PlaceableEntityProperties Copy()
        {
            return new PlaceableEntityProperties(Sprite, States, Name, Id, CanBePlacedOnType, Gui);
        }
    }
}