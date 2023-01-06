using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using GameEngine_druhypokus.Factories;
using IndustrialEngineer.Blocks;
using IndustrialEngineer.Factories;
using IndustrialEngineer.Threads;
using IndustrialEnginner.Blocks;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Enums;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Gui;
using IndustrialEnginner.Interfaces;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Drill = IndustrialEnginner.Items.Drill;

namespace IndustrialEnginner
{
    public class Game : GameLoop
    {
        // public const uint DEFAULT_WIN_WIDTH = 1200;
        // public const uint DEFAULT_WIN_HEIGHT = 900;
        public const uint DEFAULT_WIN_WIDTH = 1500;
        public const uint DEFAULT_WIN_HEIGHT = 800;
        public uint view_width = DEFAULT_WIN_WIDTH;
        public uint view_height = DEFAULT_WIN_HEIGHT;

        public const string WINDOW_TITLE = "IndustrialEnginner";

        private GuiController _guiController;

        private World _world;
        private WorldManager _worldManager;

        private ThreadManager _threadManager;

        private Zoom _zoom;
        private Moving _moving;
        private Mining _mining;
        private Vector2i _cursorWorldPos;
        private Vector2i _cursorPos;
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
                    _moving.Left = true;
                    break;
                case Keyboard.Key.W:
                    _moving.Up = true;
                    break;
                case Keyboard.Key.S:
                    _moving.Down = true;
                    break;
                case Keyboard.Key.D:
                    _moving.Right = true;
                    break;
                case Keyboard.Key.E:
                    _guiController.OpenOrClosePlayerInventory();
                    break;
                case Keyboard.Key.Escape:
                    _guiController.ClosePlayerInventory();
                    break;
                    
            }
        }

        public override void KeyReleased(object o, KeyEventArgs k)
        {
            switch (k.Code)
            {
                case Keyboard.Key.A:
                    _moving.Left = false;
                    break;
                case Keyboard.Key.W:
                    _moving.Up = false;
                    break;
                case Keyboard.Key.S:
                    _moving.Down = false;
                    break;
                case Keyboard.Key.D:
                    _moving.Right = false;
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
                    if (_guiController.Gui.State == GuiState.GamePlay)
                    {
                        SetupMining();
                    }

                    break;
                case Mouse.Button.Middle:
                    break;
                case Mouse.Button.Right:
                    if (_guiController.Gui.State == GuiState.GamePlay)
                    {
                        Build(_cursor.GetWorldPosition(Window, View, _world.TileSize, _zoom.FlippedZoomed,
                                _zoom.MaxZoom,
                                Mouse.GetPosition(Window),
                                _worldManager.MapLoader, _world.ChunkSize, _world.ChunksAroundMiddleChunks),
                            _player.Hotbar.SelectedItemSlot);
                    }

                    break;
            }
        }

        public override void OnMouseScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (e.Delta == 1 && _zoom.Zoomed < _zoom.MaxZoom)
            {
                ZoomIn();
            }

            if (e.Delta == -1 && _zoom.Zoomed > _zoom.MinZoom)
            {
                ZoomOut();
            }

            FlipZoomed();
        }

        private void ZoomOut()
        {
            view_height *= _zoom.ZoomStep;
            view_width *= _zoom.ZoomStep;
            _zoom.Zoomed /= _zoom.ZoomStep;
            View.Size = new Vector2f(view_width, view_height);
            Window.SetView(View);
        }

        private void ZoomIn()
        {
            view_height /= _zoom.ZoomStep;
            view_width /= _zoom.ZoomStep;
            _zoom.Zoomed *= _zoom.ZoomStep;
            View.Size = new Vector2f(view_width, view_height);
            Window.SetView(View);
        }

        private void FlipZoomed()
        {
            int steps = 0;
            do
            {
                steps += 1;
            } while (CalculateSequence(steps) != _zoom.Zoomed);

            _zoom.FlippedZoomed = CalculateSequence(steps, true);
        }

        private float CalculateSequence(int steps, bool reverted = false)
        {
            float revert;
            if (reverted)
            {
                revert = _zoom.MaxZoom * 2;
                for (int i = 0; i < steps; i++)
                {
                    revert /= _zoom.ZoomStep;
                }

                return revert;
            }

            revert = _zoom.MinZoom / 2;
            for (int i = 0; i < steps; i++)
            {
                revert *= _zoom.ZoomStep;
            }

            return revert;
        }

        #endregion


        public override void LoadContent()
        {
            DebugUtil.LoadContent();
        }

        public void Move()
        {
            if (CanStepOn(_moving.Left, -_moving.Step, 0))
            {
                View.Move(new Vector2f(-_moving.Step * GameTime.DeltaTime, 0));
                _player.Move((-_moving.Step * GameTime.DeltaTime) / _world.TileSize, 0);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.Up, 0, -_moving.Step))
            {
                View.Move(new Vector2f(0, -_moving.Step * GameTime.DeltaTime));
                _player.Move(0, (-_moving.Step * GameTime.DeltaTime) / _world.TileSize);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.Down, 0, _moving.Step))
            {
                View.Move(new Vector2f(0, _moving.Step * GameTime.DeltaTime));
                _player.Move(0, (_moving.Step * GameTime.DeltaTime) / _world.TileSize);
                Window.SetView(View);
            }

            if (CanStepOn(_moving.Right, _moving.Step, 0))
            {
                View.Move(new Vector2f(_moving.Step * GameTime.DeltaTime, 0));
                _player.Move((_moving.Step * GameTime.DeltaTime) / _world.TileSize, 0);
                Window.SetView(View);
            }

            _worldManager.MapLoader.Update(_player, _world.ChunkSize);
        }


        private bool CanStepOn(bool direction, float stepX, float stepY)
        {
            int futurex = (int)(_player.Properties.X + stepX * GameTime.DeltaTime / _world.TileSize);
            int futurey = (int)(_player.Properties.Y + stepY * GameTime.DeltaTime / _world.TileSize);
            return direction && _world.Map[futurex, futurey]
                       .Properties.CanStepOn && futurex > 0 && futurex < _world.MapSize - 1 && futurey > 0 &&
                   futurey < _world.MapSize - 1;
        }


        private void SetupMining()
        {
            if (_cursorWorldPos.X < 0 || _cursorWorldPos.Y < 0 || _cursorWorldPos.X > _world.MapSize ||
                _cursorWorldPos.Y > _world.MapSize)
                return;

            if (_world.Map[_cursorWorldPos.X, _cursorWorldPos.Y].Properties.Harvestable)
            {
                _mining.IsMining = true;
                _mining.MiningCoords = _cursorWorldPos;
                _mining.FinishValue = _world.Map[_cursorWorldPos.X, _cursorWorldPos.Y].Properties.HarvestTime;
                _mining.ActualProgress = 0.01f;
            }
        }

        private void Build(Vector2i pos, ItemSlot entitySlot)
        {
            if (pos.X < 0 || pos.Y < 0 || pos.X > _world.MapSize || pos.Y > _world.MapSize)
                return;
            var selectedBlock = _world.Map[pos.X, pos.Y];
            if (selectedBlock.Properties.PlacedBuilding != null)
            {
                if (selectedBlock.Properties.PlacedBuilding.Properties.Dialog != null)
                {
                    _guiController.OpenMachineDialog(selectedBlock.Properties.PlacedBuilding.Properties.Dialog);
                }

                return;
            }

            if (entitySlot == null)
                return;
            if (entitySlot.IsSelected == false)
                return;
            if (entitySlot.StorageItem == null)
                return;
            if (entitySlot.StorageItem.Item == null)
                return;
            var placingEntityType = GameData.BuildingsRegistry.Registry.Find(x =>
                x.Properties.Id == entitySlot.StorageItem.Item.Properties.PlacedBuildingId);
            var placingEntity = BuildingsFactory.MakeNewInstanceOfBuilding(placingEntityType.GetType());
            if (selectedBlock.Properties.CanPlaceOn &&
                placingEntity.Properties.CanBePlacedOnType == selectedBlock.Properties.BlockType &&
                selectedBlock.Properties.PlacedBuilding == null)
            {
                selectedBlock.PlaceEntity(placingEntity);
                placingEntity.SetPosition(pos);
                placingEntity.Properties.FoundationBlock = selectedBlock;
                entitySlot.RemoveItem(1);
                _worldManager.UpdateMap();
            }
        }

        private void Mine(Vector2i pos, int toolLevel)
        {
            if (_mining.MiningCoords != _cursorWorldPos)
            {
                SetupMining();
                return;
            }

            var selectedBlock = _world.Map[_cursorWorldPos.X, _cursorWorldPos.Y];
            _mining.ActualProgress += _mining.Speed * GameTime.DeltaTime;
            if (_mining.ActualProgress < _mining.FinishValue)
            {
                return;
            }

            _mining.IsMining = false;

            if (selectedBlock.Properties.PlacedBuilding == null)
            {
                StorageItem itemToAdd = new StorageItem()
                {
                    Item =
                        GameData.ItemRegistry.Registry.Find(x =>
                            x.Properties.Id == selectedBlock.Properties.DropId),
                    Count = selectedBlock.Properties.DropCount
                };
                StorageItem returnedStorageItem = _player.Inventory.AddItem(itemToAdd);
                if (returnedStorageItem != null)
                    returnedStorageItem = _player.Hotbar.AddItem(returnedStorageItem);
                if (returnedStorageItem == null)
                {
                    selectedBlock.Properties.Richness--;
                    selectedBlock.Properties.DropCount =
                        selectedBlock.Properties.OriginalDropCount;
                    if (selectedBlock.Properties.Richness <= 0)
                    {
                        _world.Map[_cursorWorldPos.X, _cursorWorldPos.Y] = GameData.BlockRegistry.Registry.Find(x =>
                            x.Properties.Id ==
                            selectedBlock.Properties.FoundationId);
                        _worldManager.UpdateMap();
                    }
                }
                else
                {
                    selectedBlock.Properties.DropCount =
                        returnedStorageItem.Count;
                }
            }
            else
            {
                StorageItem itemToAdd = new StorageItem()
                {
                    Item =
                        GameData.ItemRegistry.Registry.Find(x =>
                            x.Properties.Id == selectedBlock.Properties.PlacedBuilding.Properties.DropItemId),
                    Count = 1
                };
                StorageItem returnedStorageItem = _player.Hotbar.AddItem(itemToAdd);
                if (returnedStorageItem == null)
                {
                    selectedBlock.RemoveEntity();
                    _worldManager.UpdateMap();
                }
                else
                {
                    returnedStorageItem = _player.Inventory.AddItem(itemToAdd);
                    if (returnedStorageItem == null)
                    {
                        selectedBlock.RemoveEntity();
                        _worldManager.UpdateMap();
                    }
                }
            }
        }

        #region Initialization

        public override void Initialize()
        {
            InitializeGame();
            InitializeWorld();
            InitializeView();
            InitializeEntities();
            InitializeGui();
        }

        private void InitializeGui()
        {
            _guiController = new GuiController(View, Window, _zoom, _cursor);
            _player.Inventory = _guiController.Gui.Inventory;
            _player.Hotbar = _guiController.Gui.Hotbar;
            // _player.Hotbar.AddItem(new StorageItem() { Count = 3, Item = GameData.ItemRegistry.Drill.Copy() });
            // _player.Hotbar.AddItem(new StorageItem() { Count = 3, Item = GameData.ItemRegistry.Furnace.Copy() });
            // _player.Hotbar.AddItem(new StorageItem() { Count = 3, Item = GameData.ItemRegistry.WoodenPlatform.Copy() });
            // _player.Hotbar.AddItem(new StorageItem() { Count = 10, Item = GameData.ItemRegistry.Log.Copy() });
            // _player.Hotbar.AddItem(new StorageItem() { Count = 10, Item = GameData.ItemRegistry.RawIron.Copy() });
            BuildingsFactory.SetDialogsToMachines();
        }

        private void InitializeGame()
        {
            _threadManager = new ThreadManager();
            _zoom = new Zoom(2, 1, 2, 4, 0.5f);
            Window.SetMouseCursorVisible(true);
            _moving = new Moving(80);
            _mining = new Mining();
            GameData.Font = new Font(GameData.CONSOLE_FONT_PATH);
            GameData.Sprites = SpriteFactory.LoadSprites(GameData.TEXTURES_DIRECTORY_PATH);
            GameData.BlockRegistry = BlockFactory.LoadBlocks("./assest/settings/blockregistry.json");
            GameData.ItemRegistry = ItemFactory.LoadItems("./assest/settings/itemregistry.json");
            GameData.BuildingsRegistry =
                BuildingsFactory.LoadBuildings("./assest/settings/placeableEntitiesRegistry.json");
            GameData.RecipesRegistry = RecipeFactory.LoadRecipes("./assest/settings/craftingrecipies.json");
        }

        private void InitializeEntities()
        {
            GameData.GraphicsEntitiesRegistry = new GraphicsEntitiesRegistry();
            GameData.GraphicsEntitiesRegistry.Player = new GraphicsEntityProperties(GameData.Sprites["Chuck"], null);
            _player = new Player(GameData.GraphicsEntitiesRegistry.Player.Copy());
            Sprite[] progressBarStates =
            {
                GameData.Sprites["progressbar0"], GameData.Sprites["progressbar1"],
                GameData.Sprites["progressbar2"], GameData.Sprites["progressbar3"],
                GameData.Sprites["progressbar4"], GameData.Sprites["progressbar5"],
                GameData.Sprites["progressbar6"], GameData.Sprites["progressbar7"],
                GameData.Sprites["progressbar8"], GameData.Sprites["progressbar9"],
                GameData.Sprites["progressbar10"]
            };
            GameData.GraphicsEntitiesRegistry.ProgressBar =
                new GraphicsEntityProperties(progressBarStates[0], progressBarStates);
            GameData.GraphicsEntitiesRegistry.Cursor = new GraphicsEntityProperties(GameData.Sprites["selector"], null);
            _cursor = new Cursor(GameData.GraphicsEntitiesRegistry.Cursor.Copy(),
                GameData.GraphicsEntitiesRegistry.ProgressBar.Copy(), _player);
            _player.SetPosition(_world.RenderArea / 2, _world.RenderArea / 2);
        }

        private void InitializeView()
        {
            View = new View(new FloatRect(0, 0, view_width, view_height));
            View.Center = new Vector2f((_world.RenderArea / 2) * _world.TileSize,
                (_world.RenderArea / 2) * _world.TileSize);
            Window.SetView(View);
        }

        private void InitializeWorld()
        {
            _world = new World(40, 27, 32, 5);
            _worldManager = new WorldManager(_world);
            _worldManager.Initialize();
            _threadManager.StartThread(_worldManager.Updater);
        }

        #endregion

        public override void WindowClosed(object sender, EventArgs e)
        {
            _threadManager.StopAllThreads();
            base.WindowClosed(sender, e);
        }

        public override void Update(GameTime gameTime)
        {
            _cursorWorldPos = _cursor.GetWorldPosition(Window, View, _world.TileSize, _zoom.FlippedZoomed,
                _zoom.MaxZoom,
                Mouse.GetPosition(Window),
                _worldManager.MapLoader, _world.ChunkSize, _world.ChunksAroundMiddleChunks);
            _cursorPos = _cursor.GetPosition(Window, View, _world.TileSize, _zoom.FlippedZoomed, _zoom.MaxZoom,
                Mouse.GetPosition(Window));

            if (_world.LastStandXChunk != _worldManager.MapLoader.middleXChunk ||
                _world.LastStandYChunk != _worldManager.MapLoader.middleYChunk)
            {
                _worldManager.UpdateMap();
                MoveCameraAfterLoadNewWorldPart();
                _worldManager.ActualizeLastStandedChunksValues();
            }

            if (_moving.IsMoving())
            {
                Move();
            }

            if (_mining.IsMining)
            {
                Mine(_cursorWorldPos, 4);
            }

            _guiController.UpdatePosition(View, _zoom);
        }


        private void MoveCameraAfterLoadNewWorldPart()
        {
            if (_world.LastStandXChunk > _worldManager.MapLoader.middleXChunk)
            {
                View.Move(new Vector2f(_world.ChunkSize * _world.TileSize, 0));
            }
            else if (_world.LastStandXChunk < _worldManager.MapLoader.middleXChunk)
            {
                View.Move(new Vector2f(-_world.ChunkSize * _world.TileSize, 0));
            }

            if (_world.LastStandYChunk > _worldManager.MapLoader.middleYChunk)
            {
                View.Move(new Vector2f(0, _world.ChunkSize * _world.TileSize));
            }
            else if (_world.LastStandYChunk < _worldManager.MapLoader.middleYChunk)
            {
                View.Move(new Vector2f(0, -_world.ChunkSize * _world.TileSize));
            }

            Window.SetView(View);
        }

        public override void Draw(GameTime gameTime)
        {
            Window.Draw(_world.RenderedTiles);
            _worldManager.DrawEntities(Window);
            if (_guiController.Gui.State == GuiState.GamePlay)
                DebugUtil.DebugTexts[0].DisplayedString =
                    _cursor.Draw(Window, _cursorPos, _zoom, View, _guiController.Gui.State).ToString();
            _player.Draw(Window, View, _zoom);

            if (_mining.IsMining)
                _cursor._progressBar.Draw(Window, _cursorPos, _world.TileSize, _mining.FinishValue,
                    _mining.ActualProgress);


            _guiController.DrawGui(Window, _zoom);

            if (_guiController.Gui.State != GuiState.GamePlay)
                DebugUtil.DebugTexts[0].DisplayedString =
                    _cursor.Draw(Window, _cursorPos, _zoom, View, _guiController.Gui.State).ToString();
            DebugUtil.DebugTexts[1].DisplayedString =
                _guiController.GetClickedComponent(Mouse.GetPosition(Window))?.ToString();
            // DebugUtil.debugTexts[2].DisplayedString = _zoom.Zoomed.ToString();
            // msg = _zoom.FlippedZoomed.ToString();
            //msg = _mining.IsMining.ToString();
            //msg2 = _zoomed.ToString();
            // DebugUtil.debugTexts[3].DisplayedString = Mouse.GetPosition(Window).ToString();
            //msg2 = _guiController.GetGui().Inventory.Sprite.Texture.Size.ToString();
            // msg = View.Size.ToString();
            // msg2 = Mouse.GetPosition().ToString();

            // DebugUtil.DrawPerformanceData(this, Color.White, View, _zoom.FlippedZoomed);
        }
    }
}