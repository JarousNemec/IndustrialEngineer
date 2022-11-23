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
        public const float MINIMAL_TIME_BETWEEN_FRAMES = 1f / TARGET_FPS;
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

            double actualTime = 0;
            double previousTime = 0;
            double deltaTime = 0;
            double totalDeltaTime = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (Window.IsOpen)
            {
                actualTime =  SecondsFromStart(stopwatch);
                deltaTime = actualTime - previousTime;
                previousTime = actualTime;
                totalDeltaTime += deltaTime;
                
                Window.DispatchEvents();

                if (totalDeltaTime >= MINIMAL_TIME_BETWEEN_FRAMES)
                {
                    GameTime.Update((float)totalDeltaTime, (float)SecondsFromStart(stopwatch));
                    totalDeltaTime = 0;
                    Update(GameTime);

                    Window.Clear(WindowClearColor);
                    Draw(GameTime);
                    Window.Display();
                }
            }
            stopwatch.Stop();
        }

        private double SecondsFromStart(Stopwatch stopwatch)
        {
            return (double)stopwatch.ElapsedTicks / TimeSpan.TicksPerSecond;
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