using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class GraphicsEntityProperties
    {
        public Sprite Sprite { get; set; }
        public Sprite[] States { get; set; }
        public float X { get; set; }
        public float Y { get; set; }


        public GraphicsEntityProperties(Sprite sprite, Sprite[] states, float x = 0, float y = 0)
        {
            Sprite = sprite;
            States = states;
            X = x;
            Y = y;
        }

        public GraphicsEntityProperties Copy()
        {
            return new GraphicsEntityProperties(Sprite, States, X, Y);
        }


    }
}