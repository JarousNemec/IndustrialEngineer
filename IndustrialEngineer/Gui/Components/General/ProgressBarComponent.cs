using SFML.Graphics;

namespace IndustrialEnginner.Gui
{
    public class ProgressBarComponent : PictureBox
    {
        public Sprite[] States { get; set; }
        public ProgressBarComponent(Sprite sprite, Sprite[] states) : base(sprite)
        {
            States = states;
        }
        
        public static int CalculateCurrentStateIndex(int statesCount,int finishValue, float actualProgress)
        {
            var temp = finishValue / (float)(statesCount - 1);
            var index = (int)(actualProgress / temp);
            if (index > statesCount - 1)
                return statesCount - 1;
            return index;
        }
    }
}