using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus
{
    public abstract class GameLoop
    {
        public const int TARGET_FPS = 60;
        public const float TIME_UNTIL_UPDATE = 1f / TARGET_FPS;
        public RenderWindow Window { get; protected set; }

        public GameTime GameTime { get; protected set; }

        public Color WindowClearColor { get; protected set; }

        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, Color windowClearColor)
        {
            WindowClearColor = windowClearColor;
            Window = new RenderWindow(new VideoMode(windowWidth, windowHeight), windowTitle);
            GameTime = new GameTime();
            Window.Closed += WindowClosed;
            Window.KeyReleased += KeyReleased;
            Window.KeyPressed += KeyPressed;
            Window.MouseWheelScrolled += WindowOnMouseWheelScrolled;
            Window.MouseButtonPressed += WindowOnMouseButtonPressed;
        }

        public abstract void WindowOnMouseButtonPressed(object sender, MouseButtonEventArgs e);

        public abstract void WindowOnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e);

        protected GameLoop()
        {
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            float totalTimeBeforeUpdate = 0f;
            float previousTimeElapsed = 0f;
            float deltaTime = 0f;
            float totalTimeElapsed = 0f;

            Clock clock = new Clock();

            while (Window.IsOpen)
            {
                Window.DispatchEvents();
                totalTimeElapsed = clock.ElapsedTime.AsSeconds();
                deltaTime = totalTimeElapsed - previousTimeElapsed;
                previousTimeElapsed = totalTimeElapsed;

                totalTimeBeforeUpdate += deltaTime;

                if (totalTimeBeforeUpdate >= TIME_UNTIL_UPDATE)
                {
                    GameTime.Update(totalTimeBeforeUpdate, clock.ElapsedTime.AsSeconds());
                    totalTimeBeforeUpdate = 0f;
                    Update(GameTime);

                    Window.Clear(WindowClearColor);
                    Draw(GameTime);
                    Window.Display();
                }
            }
        }
        public abstract void KeyPressed(object o, KeyEventArgs k);
        public abstract void KeyReleased(object o, KeyEventArgs k);
        public abstract void LoadContent();
        public abstract void Initialize();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        private void WindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}