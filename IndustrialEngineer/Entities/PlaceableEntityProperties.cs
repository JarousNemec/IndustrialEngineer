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

        public int DropItemId { get; set; }
        public bool CanStepOn { get; set; }

        public PlaceableEntityProperties(Sprite sprite, Sprite[] states, string name, int id,
            BlockType canBePlacedOnType, int dropItemId, bool canStepOn, GuiComponent gui = null) : base(sprite, states)
        {
            Name = name;
            Id = id;
            CanBePlacedOnType = canBePlacedOnType;
            Gui = gui;
            DropItemId = dropItemId;
            CanStepOn = canStepOn;
        }

        public PlaceableEntityProperties Copy()
        {
            return new PlaceableEntityProperties(Sprite, States, Name, Id, CanBePlacedOnType,DropItemId,CanStepOn, Gui);
        }
    }
}