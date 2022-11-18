using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using SFML.Graphics;
using SFML.System;
using Font = System.Drawing.Font;

namespace IndustrialEnginner.Gui
{
    public class Label : GuiComponent
    {
        public Text Text { get; set; }
        private uint _fontSize = 12;
        private uint _criticalFontSize = 12;
        private float _textScale = 1;

        public Label(Sprite sprite, uint fontSize, string text, SFML.Graphics.Font font) : base(sprite, ComponentType.Info)
        {
            Text = new Text(text, font, CalculateFontSize(fontSize));
            Text.Color = Color.Black;
        }

        public uint CalculateFontSize(uint size)
        {
            if (size < _criticalFontSize)
            {
                uint output = size;
                do
                {
                    output *= 2;
                    _textScale /= 2;
                } while (output < _criticalFontSize);
                return output;
            }

            return size;
        }


        public override void Draw(RenderWindow window, Zoom zoom)
        {
            Text.Position = new Vector2f(DisplayingX, DisplayingY);
            Text.Scale = new Vector2f(_textScale * zoom.FlippedZoomed, _textScale * zoom.FlippedZoomed);
            window.Draw(Text);
        }
    }
}