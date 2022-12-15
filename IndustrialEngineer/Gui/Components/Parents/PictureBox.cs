using IndustrialEngineer.Enums;
using SFML.Graphics;

namespace IndustrialEnginner.Gui
{
    public class PictureBox : GuiComponent
    {
        public PictureBox(Sprite sprite) : base(sprite, ComponentType.Info)
        {
        }
    }
}