using IndustrialEnginner.Gui;
using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class PlaceableEntityProperties : GraphicsEntityProperties
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int[] CanBePlacedOn { get; set; }
        public GuiComponent Gui { get; set; }

        public PlaceableEntityProperties(Sprite sprite, Sprite[] states, string name, int id,
            int[] canBePlacedOn, GuiComponent gui = null) : base(sprite, states)
        {
            Name = name;
            Id = id;
            CanBePlacedOn = canBePlacedOn;
            Gui = gui;
        }

        public PlaceableEntityProperties Copy()
        {
            return new PlaceableEntityProperties(Sprite, States, Name, Id, CanBePlacedOn, Gui);
        }
    }
}