using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.GameEntities
{
    public class ProgressBar : GraphicsEntity
    {
        public void Draw(RenderWindow window, Vector2i pos, int tileSize, int finishValue, float actualProgress)
        {
            Sprite state = Properties.States[CalculateCurrentStateIndex(finishValue, actualProgress)];
            state.Position = new Vector2f(pos.X - (state.Texture.Size.X / 2 - tileSize / 2), pos.Y + tileSize);
            state.Scale = new Vector2f(1, 1);
            window.Draw(state);
        }

        private int CalculateCurrentStateIndex(int finishValue, float actualProgress)
        {
            var temp = finishValue / (float)(Properties.States.Length - 1);
            var index = (int)(actualProgress / temp);
            if (index > Properties.States.Length - 1)
                return Properties.States.Length - 1;
            return index;
        }

        public ProgressBar(GraphicsEntityProperties properties) : base(properties)
        {
        }
    }
}