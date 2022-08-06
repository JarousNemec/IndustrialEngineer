using System;
using System.Security.Cryptography;
using GameEngine_druhypokus.GameEntities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine_druhypokus
{
    public class Game : GameLoop
    {
        public const uint DEFAULT_WIN_WIDTH = 1200;
        public const uint DEFAULT_WIN_HEIGHT = 900;
        public uint view_width = DEFAULT_WIN_WIDTH / 2;
        public uint view_height = DEFAULT_WIN_HEIGHT / 2;
        public const string WINDOW_TITLE = "Game";
        public GameData GameData;
        private Tilemap map;
        private float step = 80;

        private View View;
        private bool left;
        private bool right;
        private bool up;
        private bool down;
        private Player _player;

        public bool move = false;

        public Game() : base(DEFAULT_WIN_WIDTH, DEFAULT_WIN_HEIGHT, WINDOW_TITLE, Color.Black)
        {
        }

        public override void KeyPressed(object o, KeyEventArgs k)
        {
            switch (k.Code)
            {
                case Keyboard.Key.A:
                    left = true;
                    break;
                case Keyboard.Key.W:
                    up = true;
                    break;
                case Keyboard.Key.S:
                    down = true;
                    break;
                case Keyboard.Key.D:
                    right = true;
                    break;
            }
        }

        public override void KeyReleased(object o, KeyEventArgs k)
        {
            switch (k.Code)
            {
                case Keyboard.Key.A:
                    left = false;
                    break;
                case Keyboard.Key.W:
                    up = false;
                    break;
                case Keyboard.Key.S:
                    down = false;
                    break;
                case Keyboard.Key.D:
                    right = false;
                    break;
            }
        }

        public override void LoadContent()
        {
            DebugUtil.LoadContent();
        }

        private string msg = "----";

        public void Move()
        {
            if (left)
            {
                View.Move(new Vector2f(-step * GameTime.DeltaTime, 0));
                Window.SetView(View);
            }

            if (up)
            {
                View.Move(new Vector2f(0, -step * GameTime.DeltaTime));
                Window.SetView(View);
            }

            if (down)
            {
                View.Move(new Vector2f(0, step * GameTime.DeltaTime));
                Window.SetView(View);
            }

            if (right)
            {
                View.Move(new Vector2f(step * GameTime.DeltaTime, 0));
                Window.SetView(View);
            }

            //msg = _player.Sprite.Position.ToString();
            msg = View.Center.ToString();
        }

        private uint zoom = 2;

        public override void WindowOnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta == 1)
            {
                view_height /= zoom;
                view_width /= zoom;
            }
            else
            {
                view_height *= zoom;
                view_width *= zoom;
            }
            View.Size = new Vector2f(view_width,view_height);
            msg = zoom + " X";
            Window.SetView(View);
        }

        public override void Initialize()
        {
            GameData = new GameData();
            // int[] level =
            // {
            //     0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            //     0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0,
            //     1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3,
            //     0, 1, 0, 0, 2, 0, 3, 3, 3, 0, 1, 1, 1, 0, 0, 0,
            //     0, 1, 1, 0, 3, 3, 3, 0, 0, 0, 1, 1, 1, 2, 0, 0,
            //     0, 0, 1, 0, 3, 0, 2, 2, 0, 0, 1, 1, 1, 1, 2, 0,
            //     2, 0, 1, 0, 3, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,
            //     0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1,
            //     0, 0, 1, 0, 3, 2, 2, 0, 0, 0, 0, 0, 1, 1, 1, 1,
            //     0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            //     0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 2, 0, 0, 0, 0,
            //     1, 1, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 3,
            //     0, 1, 0, 0, 2, 0, 3, 3, 3, 0, 1, 1, 1, 0, 0, 0,
            //     0, 1, 1, 0, 3, 3, 3, 0, 0, 0, 1, 1, 1, 2, 0, 0,
            //     0, 0, 1, 0, 3, 0, 2, 2, 0, 0, 1, 1, 1, 1, 2, 0,
            //     2, 0, 1, 0, 3, 0, 2, 2, 2, 0, 1, 1, 1, 1, 1, 1,
            //     0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1,
            //     0, 0, 1, 0, 3, 2, 2, 2, 0, 0, 0, 0, 1, 1, 1, 1,
            // };
            Random r = new Random();
            var generator = new MapGenerator();
            int mapSize = 1000;
            int[] level = generator.Generate(mapSize, r.Next(1,9999));
            map = new Tilemap();
            map.load(new Vector2u(32, 32), level, (uint)mapSize, (uint)mapSize);
            View = new View(new FloatRect(0, 0, view_width, view_height));
            View.Center = new Vector2f(200, 200);
            Window.SetView(View);
            _player = new Player(GameData.GetSprites()["player"]);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }

        public override void Draw(GameTime gameTime)
        {
            Window.Draw(map);
            _player.Draw(Window, View);
            DebugUtil.DrawPerformanceData(this, Color.White, View, msg);
        }
    }
}