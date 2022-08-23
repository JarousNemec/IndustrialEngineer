using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner
{
    public class Game : GameLoop
    {
        public const uint DEFAULT_WIN_WIDTH = 1200;
        public const uint DEFAULT_WIN_HEIGHT = 900;
        public uint view_width = DEFAULT_WIN_WIDTH / 2;
        public uint view_height = DEFAULT_WIN_HEIGHT / 2;
        public const string WINDOW_TITLE = "Game";
        public GameData GameData;
        private BlockRegistry _blockRegistry;
        private MapLoader _mapLoader;
        private Tilemap map;
        private float step = 80;

        public uint zoom = 2;
        public static int chunkSize = 40;
        public int mapSize = chunkSize * 27;
        public int tileSize = 32;
        public int renderArea = chunkSize * 3;


        private Moving _moving;
        private bool _mining = false;
        
        private View View;
        private Player _player;
        private Cursor _cursor;

        public Game() : base(DEFAULT_WIN_WIDTH, DEFAULT_WIN_HEIGHT, WINDOW_TITLE, Color.Black)
        {
        }

        public override void KeyPressed(object o, KeyEventArgs k)
        {
            switch (k.Code)
            {
                case Keyboard.Key.A:
                    _moving.left = true;
                    break;
                case Keyboard.Key.W:
                    _moving.up = true;
                    break;
                case Keyboard.Key.S:
                    _moving.down = true;
                    break;
                case Keyboard.Key.D:
                    _moving.right = true;
                    break;
            }
        }

        public override void KeyReleased(object o, KeyEventArgs k)
        {
            switch (k.Code)
            {
                case Keyboard.Key.A:
                    _moving.left = false;
                    break;
                case Keyboard.Key.W:
                    _moving.up = false;
                    break;
                case Keyboard.Key.S:
                    _moving.down = false;
                    break;
                case Keyboard.Key.D:
                    _moving.right = false;
                    break;
            }
        }

        public override void LoadContent()
        {
            DebugUtil.LoadContent();
        }

        private string msg = "----";
        private string msg2 = "----";

        public void Move()
        {
            if (CanStepOn(_moving.left, -step, 0))
            {
                View.Move(new Vector2f(-step * GameTime.DeltaTime, 0));
                _player.Move((-step * GameTime.DeltaTime) / tileSize, 0);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.up, 0, -step))
            {
                View.Move(new Vector2f(0, -step * GameTime.DeltaTime));
                _player.Move(0, (-step * GameTime.DeltaTime) / tileSize);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.down, 0, step))
            {
                View.Move(new Vector2f(0, step * GameTime.DeltaTime));
                _player.Move(0, (step * GameTime.DeltaTime) / tileSize);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.right, step, 0))
            {
                View.Move(new Vector2f(step * GameTime.DeltaTime, 0));
                _player.Move((step * GameTime.DeltaTime) / tileSize, 0);
                Window.SetView(View);
            }

            _mapLoader.Update(_player, chunkSize);
        }

        private int futurex = 0;
        private int futurey = 0;

        private bool CanStepOn(bool direction, float stepX, float stepY)
        {
            futurex = (int)(_player.GetX() + stepX * GameTime.DeltaTime / tileSize);
            futurey = (int)(_player.GetY() + stepY * GameTime.DeltaTime / tileSize);
            return direction && level[futurex, futurey]
                .CanStepOn && futurex > 0 && futurex < mapSize - 1 && futurey > 0 && futurey < mapSize - 1;
        }

        private short zoomed = 1;
        private short minZoom = 4;

        public override void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    _mining = false;
                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    break;
            }
        }

        public override void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    _mining = true;
                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    Build(_cursor.GetWorldPosition(Window, View, tileSize, zoomed, minZoom, Mouse.GetPosition(Window),
                        _mapLoader, chunkSize), _blockRegistry.Bridge.Copy());
                    break;
            }
        }

        private void Build(Vector2i pos, Block block)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X > mapSize || pos.Y > mapSize)
                return;

            if (level[pos.X, pos.Y].CanPlaceOn && level[pos.X, pos.Y].BlocksCanPlaceOn.Contains(block.Id))
            {
                level[pos.X, pos.Y] = block;
                renderedPart = _mapLoader.GetCurrentChunks(level, mapSize, renderArea, _player, chunkSize);
                map.load(new Vector2u((uint)tileSize, (uint)tileSize), renderedPart, (uint)renderArea,
                    (uint)renderArea);
            }
        }

        private void Destroy(Vector2i pos, int toolLevel)
        {
        }

        public override void OnMouseScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta == 1 && zoomed > 0)
            {
                view_height /= zoom;
                view_width /= zoom;
                zoomed--;
                View.Size = new Vector2f(view_width, view_height);
                Window.SetView(View);
            }

            if (e.Delta == -1 && zoomed + 1 != minZoom)
            {
                view_height *= zoom;
                view_width *= zoom;
                zoomed++;
                View.Size = new Vector2f(view_width, view_height);
                Window.SetView(View);
            }
        }

        private Block[,] level;
        private Block[,] renderedPart;
        private int lastStandXChunk = 1;
        private int lastStandYChunk = 1;

        #region Initialization

        public override void Initialize()
        {
            InitializeGame();
            InitializeWorld();
            InitializeTilemap();
            InitializeView();
            InitializeEntities();
        }

        private void InitializeGame()
        {
            Window.SetMouseCursorVisible(false);
            _moving = new Moving();
            GameData = new GameData();
            _blockRegistry = BlockFactory.LoadBlocks("blockregistry.json");
        }

        private void InitializeEntities()
        {
            _player = new Player(GameData.GetSprites()["Chuck"]);
            _cursor = new Cursor(GameData.GetSprites()["selector"], _player, GameData.GetSprites()["progressbar0"]);
            _player.SetPosition(renderArea / 2, renderArea / 2);
        }

        private void InitializeView()
        {
            View = new View(new FloatRect(0, 0, view_width, view_height));
            View.Center = new Vector2f((renderArea / 2) * tileSize, (renderArea / 2) * tileSize);
            Window.SetView(View);
        }

        private void InitializeTilemap()
        {
            map = new Tilemap();
            map.load(new Vector2u((uint)tileSize, (uint)tileSize), renderedPart, (uint)renderArea, (uint)renderArea);
        }

        private void InitializeWorld()
        {
            Random r = new Random();
            var generator = new MapGenerator(_blockRegistry);
            _mapLoader = new MapLoader();
            if (renderArea > mapSize)
            {
                renderArea = mapSize;
            }

            lastStandXChunk = _mapLoader.middleXChunk;
            lastStandYChunk = _mapLoader.middleYChunk;
            level = generator.Generate(mapSize, r.Next(1, 99999999));
            renderedPart = _mapLoader.GetCurrentChunks(level, mapSize, renderArea, _player, chunkSize);
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (lastStandXChunk != _mapLoader.middleXChunk || lastStandYChunk != _mapLoader.middleYChunk)
            {
                renderedPart = _mapLoader.GetCurrentChunks(level, mapSize, renderArea, _player, chunkSize);
                map.load(new Vector2u((uint)tileSize, (uint)tileSize), renderedPart, (uint)renderArea,
                    (uint)renderArea);
                MoveCameraAfterLoadNewWorldPart();
                ActualizeLastStandedChunksValues();
            }

            if (_moving.IsMoving())
            {
                Move();
            }

            if (_mining)
            {
                Destroy(_cursor.GetWorldPosition(Window, View, tileSize, zoomed, minZoom, Mouse.GetPosition(Window),
                    _mapLoader, chunkSize),4);
            }
        }

        private void ActualizeLastStandedChunksValues()
        {
            lastStandXChunk = _mapLoader.middleXChunk;
            lastStandYChunk = _mapLoader.middleYChunk;
        }

        private void MoveCameraAfterLoadNewWorldPart()
        {
            if (lastStandXChunk > _mapLoader.middleXChunk)
            {
                View.Move(new Vector2f(chunkSize * tileSize, 0));
            }
            else if (lastStandXChunk < _mapLoader.middleXChunk)
            {
                View.Move(new Vector2f(-chunkSize * tileSize, 0));
            }

            if (lastStandYChunk > _mapLoader.middleYChunk)
            {
                View.Move(new Vector2f(0, chunkSize * tileSize));
            }
            else if (lastStandYChunk < _mapLoader.middleYChunk)
            {
                View.Move(new Vector2f(0, -chunkSize * tileSize));
            }

            Window.SetView(View);
        }

        public override void Draw(GameTime gameTime)
        {
            Window.Draw(map);
            _player.Draw(Window, View);
            var cpos = _cursor.GetPosition(Window, View, tileSize, zoomed, minZoom, Mouse.GetPosition(Window));
            msg2 = _cursor.Draw(Window, cpos);
            //msg = zoomed.ToString();
            msg = _mining.ToString();
            DebugUtil.DrawPerformanceData(this, Color.White, View, msg, msg2);
        }
    }
}