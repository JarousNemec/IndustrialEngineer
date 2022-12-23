using IndustrialEngineer.Enums;
using SFML.Graphics;

namespace IndustrialEnginner.Gui
{
    public class ClickableComponent : GuiComponent
    {
        public ClickGrid ClickGrid { get; set; }

        public ClickableComponent(Sprite sprite, ComponentType type, int rows, int columns) : base(sprite, type)
        {
            ClickGrid = new ClickGrid();
            ClickGrid.Rows = rows;
            ClickGrid.Columns = columns;
        }
    }
}