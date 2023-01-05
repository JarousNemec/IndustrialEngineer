using System;
using System.Collections.Generic;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner
{
    public static class DebugUtil
    {
        private const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";

        private static Font _consoleFont;
        public static Text[] DebugTexts = new Text[10];

        public static void LoadContent()
        {
            _consoleFont = new Font(CONSOLE_FONT_PATH);
            for (int i = 0; i < DebugTexts.Length; i++)
            {
                DebugTexts[i] = new Text(String.Empty, DebugUtil._consoleFont, 16);
                DebugTexts[i].Color = Color.White;
            }
        }


        public static void DrawPerformanceData(GameLoop gameLoop, Color fontColor, View view, float revertedZoomed)
        {
            if (_consoleFont == null)
                return;
            string totalTimeElapsed = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTime = gameLoop.GameTime.DeltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            Text textA = new Text(totalTimeElapsed, _consoleFont, 16);
            textA.Position = new Vector2f((view.Center.X - view.Size.X / 2) + 4f * revertedZoomed,
                (view.Center.Y - view.Size.Y / 2) + 4f * revertedZoomed);
            textA.Scale = new Vector2f(revertedZoomed, revertedZoomed);
            textA.Color = fontColor;

            Text textB = new Text(deltaTime, _consoleFont, 16);
            textB.Position = new Vector2f((view.Center.X - view.Size.X / 2) + 4f * revertedZoomed,
                (view.Center.Y - view.Size.Y / 2) + 24f * revertedZoomed);
            textB.Scale = new Vector2f(revertedZoomed, revertedZoomed);
            textB.Color = fontColor;

            Text textC = new Text(fpsStr, _consoleFont, 16);
            textC.Position = new Vector2f((view.Center.X - view.Size.X / 2) + 4f * revertedZoomed,
                (view.Center.Y - view.Size.Y / 2) + 44f * revertedZoomed);
            textC.Scale = new Vector2f(revertedZoomed, revertedZoomed);
            textC.Color = fontColor;

            gameLoop.Window.Draw(textA);
            gameLoop.Window.Draw(textB);
            gameLoop.Window.Draw(textC);

            float debugTextsStartY = 64f;
            for (int i = 0; i < DebugTexts.Length; i++)
            {
                var text = DebugTexts[i];
                text.Position = new Vector2f((view.Center.X - view.Size.X / 2) + 4f * revertedZoomed,
                    (view.Center.Y - view.Size.Y / 2) +
                    (debugTextsStartY + i * 20) * revertedZoomed);
                text.Scale = new Vector2f(revertedZoomed, revertedZoomed);
                // if (text.DisplayedString != String.Empty)
                gameLoop.Window.Draw(text);
            }
        }
    }
}