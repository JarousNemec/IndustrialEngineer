using IndustrialEnginner.DataModels;
using SFML.Graphics;

namespace IndustrialEnginner.Interfaces
{
    public interface IGuiComponent
    { 
        void Draw(RenderWindow window, Zoom zoom);
    }
}