using SFML.Graphics;

namespace IndustrialEnginner.Interfaces
{
    public interface IGuiComponent
    { 
        void Draw(RenderWindow window, float zoomed);
    }
}