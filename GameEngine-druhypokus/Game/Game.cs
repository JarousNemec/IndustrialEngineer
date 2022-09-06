using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GameEngine_druhypokus.Factories;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Gui;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace IndustrialEnginner
{
    public class Game : GameLoop
    {
        public const uint DEFAULT_WIN_WIDTH = 1200;
        public const uint DEFAULT_WIN_HEIGHT = 900;
        public uint view_width = DEFAULT_WIN_WIDTH;
        public uint view_height = DEFAULT_WIN_HEIGHT;
        public const string WINDOW_TITLE = "Game";
        public GameData GameData;
        private BlockRegistry _blockRegistry;
        private ItemRegistry _itemRegistry;
        private MapLoader _mapLoader;
        private Tilemap map;
        private GuiManager _guiManager;
        private float step = 80;

        public static uint zoom = 2;
        public static int chunkSize = 40;
        public int mapSize = chunkSize * 27;
        public int tileSize = 32;
        public int renderArea = chunkSize * 3;


        private Moving _moving;
        private Mining _mining;
        private Vector2i _cursorWorldPos;
        private Vector2i _cursorPos;
        private int LogCount = 0;
        private View View;
        private Player _player;
        private Cursor _cursor;

        public Game() : base(DEFAULT_WIN_WIDTH, DEFAULT_WIN_HEIGHT, WINDOW_TITLE, Color.Black)
        {
        }

        #region Controls

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
                case Keyboard.Key.E:
                    _guiManager.OpenOrClosePlayerInventory();
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

        public override void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    _mining.IsMining = false;
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
                    if (_guiManager.GetGuiState() == GuiState.GamePlay)
                    {
                        SetupMining();
                    }
                    else if (_guiManager.GetGuiState() == GuiState.OpenPlayerInventory)
                    {
                        msg = _guiManager.ClickOnGuiComponent(Mouse.GetPosition(Window), Window);
                    }

                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    Build(_cursor.GetWorldPosition(Window, View, tileSize, zoomed, minZoom, Mouse.GetPosition(Window),
                        _mapLoader, chunkSize), _blockRegistry.Bridge.Copy());
                    break;
            }
        }

        public override void OnMouseScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta == 1 && zoomed > 0)
            {
                ZoomIn();
            }

            if (e.Delta == -1 && zoomed + 1 != minZoom)
            {
                ZoomOut();
            }
        }

        #endregion

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

        private int _futurex = 0;
        private int _futurey = 0;

        private bool CanStepOn(bool direction, float stepX, float stepY)
        {
            _futurex = (int)(_player.GetX() + stepX * GameTime.DeltaTime / tileSize);
            _futurey = (int)(_player.GetY() + stepY * GameTime.DeltaTime / tileSize);
            return direction && level[_futurex, _futurey]
                .CanStepOn && _futurex > 0 && _futurex < mapSize - 1 && _futurey > 0 && _futurey < mapSize - 1;
        }

        private short zoomed = 2;
        private short minZoom = 4;


        private void SetupMining()
        {
            if (_cursorWorldPos.X < 0 || _cursorWorldPos.Y < 0 || _cursorWorldPos.X > mapSize ||
                _cursorWorldPos.Y > mapSize)
                return;

            if (level[_cursorWorldPos.X, _cursorWorldPos.Y].Harvestable)
            {
                _mining.IsMining = true;
                _mining.MiningCoords = _cursorWorldPos;
                _mining.FinishValue = level[_cursorWorldPos.X, _cursorWorldPos.Y].HarvestTime;
                _mining.ActualProgress = 0.01f;
            }
        }

        private void Build(Vector2i pos, Block block)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X > mapSize || pos.Y > mapSize)
                return;

            if (level[pos.X, pos.Y].CanPlaceOn && level[pos.X, pos.Y].BlocksCanPlaceOn.Contains(block.Id))
            {
                level[pos.X, pos.Y] = block;
                UpdateMap();
            }
        }

        private void Mine(Vector2i pos, int toolLevel)
        {
            if (_mining.MiningCoords != _cursorWorldPos)
            {
                SetupMining();
                return;
            }

            _mining.ActualProgress += _mining.speed * GameTime.DeltaTime;
            if (_mining.ActualProgress > _mining.FinishValue)
            {
                _mining.IsMining = false;
                level[_cursorWorldPos.X, _cursorWorldPos.Y].Richness--;

                if (level[_cursorWorldPos.X, _cursorWorldPos.Y].DropId == _itemRegistry.Log.Id)
                {
                    LogCount += level[_cursorWorldPos.X, _cursorWorldPos.Y].DropCount;
                }

                if (level[_cursorWorldPos.X, _cursorWorldPos.Y].Richness == 0)
                {
                    level[_cursorWorldPos.X, _cursorWorldPos.Y] = _blockRegistry.Registry.Find(x =>
                        x.Id == level[_cursorWorldPos.X, _cursorWorldPos.Y].FoundationId);
                    UpdateMap();
                }
            }
        }

        private void UpdateMap()
        {
            renderedPart = _mapLoader.GetCurrentChunks(level, mapSize, renderArea, _player, chunkSize);
            map.load(new Vector2u((uint)tileSize, (uint)tileSize), renderedPart, (uint)renderArea,
                (uint)renderArea);
        }


        private void ZoomOut()
        {
            view_height *= zoom;
            view_width *= zoom;
            zoomed++;
            View.Size = new Vector2f(view_width, view_height);
            Window.SetView(View);
        }

        private void ZoomIn()
        {
            view_height /= zoom;
            view_width /= zoom;
            zoomed--;
            View.Size = new Vector2f(view_width, view_height);
            Window.SetView(View);
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
            InitializeGui();
        }

        private void InitializeGui()
        {
            _guiManager = new GuiManager(GameData, View, _itemRegistry, Window, (int)zoom);
        }

        private void InitializeGame()
        {
            Window.SetMouseCursorVisible(true);
            _moving = new Moving();
            _mining = new Mining();
            GameData = new GameData();
            _blockRegistry = BlockFactory.LoadBlocks("blockregistry.json");
            _itemRegistry = ItemFactory.LoadItems("itemregistry.json", GameData);
        }

        private void InitializeEntities()
        {
            _player = new Player(GameData.GetSprites()["Chuck"]);
            Sprite[] progressBarStates =
            {
                GameData.GetSprites()["progressbar0"], GameData.GetSprites()["progressbar1"],
                GameData.GetSprites()["progressbar2"], GameData.GetSprites()["progressbar3"],
                GameData.GetSprites()["progressbar4"], GameData.GetSprites()["progressbar5"],
                GameData.GetSprites()["progressbar6"], GameData.GetSprites()["progressbar7"],
                GameData.GetSprites()["progressbar8"], GameData.GetSprites()["progressbar9"],
                GameData.GetSprites()["progressbar10"]
            };
            _cursor = new Cursor(GameData.GetSprites()["selector"], _player, progressBarStates);
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
            _cursorWorldPos = _cursor.GetWorldPosition(Window, View, tileSize, zoomed, minZoom,
                Mouse.GetPosition(Window),
                _mapLoader, chunkSize);
            _cursorPos = _cursor.GetPosition(Window, View, tileSize, zoomed, minZoom, Mouse.GetPosition(Window));

            if (lastStandXChunk != _mapLoader.middleXChunk || lastStandYChunk != _mapLoader.middleYChunk)
            {
                UpdateMap();
                MoveCameraAfterLoadNewWorldPart();
                ActualizeLastStandedChunksValues();
            }

            if (_moving.IsMoving())
            {
                Move();
            }

            if (_mining.IsMining)
            {
                Mine(_cursorWorldPos, 4);
            }

            _guiManager.UpdatePosition(View, zoomed);
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

            _cursor.Draw(Window, _cursorPos);
            if (_mining.IsMining)
                _cursor._progressBar.Draw(Window, _cursorPos, tileSize, _mining.FinishValue, _mining.ActualProgress);
            _guiManager.DrawGui(Window, zoomed);
            msg2 = View.Size.ToString();
            //msg = zoomed.ToString();
            //msg = LogCount.ToString() + " Logs";
            //msg = _mining.IsMining.ToString();
            //msg2 = zoomed.ToString();
            //msg2 = Mouse.GetPosition(Window).ToString();
            //msg2 = _guiManager.GetGui().Inventory.Sprite.Texture.Size.ToString();
            //msg = View.Size.ToString();
            //msg2 = Mouse.GetPosition(Window).ToString();
            DebugUtil.DrawPerformanceData(this, Color.White, View, msg, msg2, zoomed > 2 ? (zoomed+1) : zoomed);
        }
    }
}