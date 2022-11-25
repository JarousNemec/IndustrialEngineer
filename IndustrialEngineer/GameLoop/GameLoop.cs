using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner
{
    public abstract class GameLoop
    {
        public const int TARGET_FPS = 60;
        public const long MINIMAL_TIME_BETWEEN_FRAMES = TimeSpan.TicksPerSecond / TARGET_FPS;
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
            Window.MouseWheelScrolled += OnMouseScrolled;
            Window.MouseButtonPressed += OnMousePressed;
            Window.MouseButtonReleased += OnMouseReleased;
        }

        public abstract void OnMouseReleased(object sender, MouseButtonEventArgs e);

        public abstract void OnMousePressed(object sender, MouseButtonEventArgs e);

        public abstract void OnMouseScrolled(object sender, MouseWheelScrollEventArgs e);

        protected GameLoop()
        {
        }

        public void Run()
        {
            LoadContent();
            Initialize();

            long actualTime;
            long previousTime = 0;
            long deltaTime;
            long totalDeltaTime = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (Window.IsOpen)
            {
                actualTime =  stopwatch.ElapsedTicks;
                deltaTime = actualTime - previousTime;
                previousTime = actualTime;
                totalDeltaTime += deltaTime;
                
                Window.DispatchEvents();

                if (totalDeltaTime < MINIMAL_TIME_BETWEEN_FRAMES) continue;
                GameTime.Update(SecondsFromTicks(totalDeltaTime), SecondsFromTicks(stopwatch.ElapsedTicks));
                totalDeltaTime = 0;
                Update(GameTime);

                Window.Clear(WindowClearColor);
                Draw(GameTime);
                Window.Display();
            }
            stopwatch.Stop();
        }

        private float SecondsFromTicks(long ticks)
        {
            return (float)ticks / TimeSpan.TicksPerSecond;
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