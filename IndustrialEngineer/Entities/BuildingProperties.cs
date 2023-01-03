using IndustrialEngineer.Blocks;
using IndustrialEnginner.Enums;
using IndustrialEnginner.Gui;
using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class BuildingProperties : GraphicsEntityProperties
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public BlockType CanBePlacedOnType { get; set; }
        public MachineDialog Dialog { get; set; }
        public int DropItemId { get; set; }
        public bool CanStepOn { get; set; }

        public Block FoundationBlock { get; set; }

        public BuildingProperties(Sprite sprite, Sprite[] states, string name, int id,
            BlockType canBePlacedOnType, int dropItemId, bool canStepOn, MachineDialog dialog) : base(sprite, states)
        {
            Name = name;
            Id = id;
            CanBePlacedOnType = canBePlacedOnType;
            Dialog = dialog;
            DropItemId = dropItemId;
            CanStepOn = canStepOn;
        }

        public BuildingProperties Copy()
        {
            return new BuildingProperties(Sprite, States, Name, Id, CanBePlacedOnType,DropItemId,CanStepOn, Dialog?.Copy());
        }
    }
}