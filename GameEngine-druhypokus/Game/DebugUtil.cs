using System;
using System.ComponentModel;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner
{
    public static class DebugUtil
    {
        public const string CONSOLE_FONT_PATH = "./fonts/arial.ttf";

        public static Font consoleFont;

        public static void LoadContent()
        {
            consoleFont = new Font(CONSOLE_FONT_PATH);
        }

        public static void DrawPerformanceData(GameLoop gameLoop, Color fontColor, View view, string msg, string msg2, float zoomed)
        {
            if (consoleFont == null)
                return;
            if(zoomed == 0)
                zoomed = 0.5f;
            string totalTimeElapsed = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTime = gameLoop.GameTime.DeltaTime.ToString("0.00000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string fpsStr = fps.ToString("0.00");

            Text textA = new Text(totalTimeElapsed, consoleFont, 16);
            textA.Position = new Vector2f((view.Center.X-view.Size.X/2) + 4f*zoomed, (view.Center.Y-view.Size.Y/2) + 4f*zoomed);
            textA.Scale = new Vector2f(zoomed, zoomed);
            textA.Color = fontColor;

            Text textB = new Text(deltaTime, consoleFont, 16);
            textB.Position = new Vector2f((view.Center.X-view.Size.X/2) + 4f*zoomed, (view.Center.Y-view.Size.Y/2) + 24f*zoomed);
            textB.Scale = new Vector2f(zoomed, zoomed);
            textB.Color = fontColor;

            Text textC = new Text(fpsStr, consoleFont, 16);
            textC.Position = new Vector2f((view.Center.X-view.Size.X/2) + 4f*zoomed, (view.Center.Y-view.Size.Y/2) + 44f*zoomed);
            textC.Scale = new Vector2f(zoomed, zoomed);
            textC.Color = fontColor;
            
            Text textD = new Text(msg, DebugUtil.consoleFont, 16);
            textD.Position = new Vector2f((view.Center.X-view.Size.X/2) + 4f*zoomed, (view.Center.Y-view.Size.Y/2) + 64f*zoomed);
            textD.Scale = new Vector2f(zoomed, zoomed);
            textD.Color = Color.White;
            
            Text textE = new Text(msg2, DebugUtil.consoleFont, 16);
            textE.Position = new Vector2f((view.Center.X-view.Size.X/2) + 4f*zoomed, (view.Center.Y-view.Size.Y/2) + 84f*zoomed);
            textE.Scale = new Vector2f(zoomed, zoomed);
            textE.Color = Color.White;

            gameLoop.Window.Draw(textA);
            gameLoop.Window.Draw(textB);
            gameLoop.Window.Draw(textC);
            gameLoop.Window.Draw(textD);
            gameLoop.Window.Draw(textE);
        }
    }
}