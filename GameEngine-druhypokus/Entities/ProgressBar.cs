using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.GameEntities
{
    public class ProgressBar : Entity
    {
        public int StartState { get; set; } = 0;
        public int FinishState { get; set; } = 10;
        public new Sprite[] States { get; set; }

        public void Draw(RenderWindow window,Vector2i pos, int tileSize, int finishValue, float actualProgress)
        {
            Sprite state = States[CalculateCurrentStateIndex(finishValue,actualProgress)];
            state.Position = new Vector2f(pos.X-(state.Texture.Size.X/2-tileSize/2), pos.Y+tileSize);
            state.Scale = new Vector2f(1, 1);
            window.Draw(state);
        }

        private int CalculateCurrentStateIndex(int finishValue, float actualProgress)
        {
            var temp = finishValue / (float)(States.Length - 1);
            var index = (int)(actualProgress / temp);
            if (index > States.Length - 1)
                return States.Length - 1;
            return index;
        }
        public ProgressBar(Sprite[] states) : base(states[0])
        {
            States = states;
        }
    }
}