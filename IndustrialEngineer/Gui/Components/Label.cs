using SFML.Graphics;
using SFML.System;
using Font = System.Drawing.Font;

namespace IndustrialEnginner.Gui
{
    public class Label : GuiComponent
    {
        public Text Text { get; set; }

        public Label(Sprite sprite) : base(sprite)
        {
        }

        private static uint fontSize = 16;
        private static float requiredFontSize = 8;
        private static float fontSizeCorrection = requiredFontSize / fontSize;
        private float minfontCellSize = fontSize / 2 * fontSizeCorrection;

        public override void Draw(RenderWindow window, float zoomed)
        {
            Text.Position = new Vector2f(DisplayingX, DisplayingY);
            Text.Scale = new Vector2f(fontSizeCorrection * zoomed, fontSizeCorrection * zoomed);
            window.Draw(Text);
        }
    }
}