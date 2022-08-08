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
                _player.Move(-step * GameTime.DeltaTime, 0);
                Window.SetView(View);
            }

            if (up)
            {
                View.Move(new Vector2f(0, -step * GameTime.DeltaTime));
                _player.Move(0, -step * GameTime.DeltaTime);
                Window.SetView(View);
            }

            if (down)
            {
                View.Move(new Vector2f(0, step * GameTime.DeltaTime));
                _player.Move(0, step * GameTime.DeltaTime);
                Window.SetView(View);
            }

            if (right)
            {
                View.Move(new Vector2f(step * GameTime.DeltaTime, 0));
                _player.Move(step * GameTime.DeltaTime, 0);
                Window.SetView(View);
            }

            //_player.SetPosition(View.Center.X, View.Center.Y);
        }

        private uint zoom = 2;
        private short zoomed = 1;

        public override void WindowOnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta == 1 && zoomed > 0)
            {
                view_height /= zoom;
                view_width /= zoom;
                zoomed--;
            }
            if(e.Delta == -1 && zoomed < 2)
            {
                view_height *= zoom;
                view_width *= zoom;
                zoomed++;
            }
            View.Size = new Vector2f(view_width,view_height);
            //msg = zoomed + " X";
            Window.SetView(View);
        }

        public override void Initialize()
        {
            GameData = new GameData();
            Random r = new Random();
            var generator = new MapGenerator();
            var mapRender = new MapRender();
            int chunkSize = 40;
            int mapSize = chunkSize*25;
            int tileSize = 32;
            int renderArea = chunkSize*3;
            int[] level = generator.Generate(mapSize, r.Next(1,9999));
            int[] renderedPart = mapRender.GetCurrentChunks(level,mapSize,renderArea);
            map = new Tilemap();
            map.load(new Vector2u((uint)tileSize, (uint)tileSize), renderedPart, (uint)renderArea, (uint)renderArea);
            View = new View(new FloatRect(0, 0, view_width, view_height));
            View.Center = new Vector2f((renderArea/2)*tileSize, (renderArea/2)*tileSize);
            Window.SetView(View);
            _player = new Player(GameData.GetSprites()["player"]);
            _player.SetPosition((renderArea/2)*tileSize, (renderArea/2)*tileSize);
        }

        public override void Update(GameTime gameTime)
        {
            Move();
        }

        public override void Draw(GameTime gameTime)
        {
            Window.Draw(map);
            _player.Draw(Window);
            msg = _player.PrintPosition();
            DebugUtil.DrawPerformanceData(this, Color.White, View, msg);
        }
    }
}