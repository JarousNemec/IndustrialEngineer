using System.Collections.Generic;
using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class MachineDialog : GuiComponent
    {
        private GameData _gameData;
        private Sprite[] _states;
        public ItemStorage[] ItemStorages { get; set; }
        public Vector2i[] StoragesPositionsInDialog { get; set; }

        public MachineDialog(Sprite[] states, GameData gameData, ItemStorage[] itemStorages, Vector2i[] storagesPositionsInDialog) : base(states[0],
            ComponentType.Interface)
        {
            _states = states;
            _gameData = gameData;
            ItemStorages = itemStorages;
            StoragesPositionsInDialog = storagesPositionsInDialog;
            foreach (var itemStorage in itemStorages)
            {
                _childComponentsToDraw.Add(itemStorage);
            }
        }

        public override void Draw(RenderWindow window, Zoom zoom)
        {
            base.Draw(window, zoom);
        }

        public MachineDialog Copy()
        {
            ItemStorage[] storages = new ItemStorage[ItemStorages.Length];
            for (int i = 0; i < storages.Length; i++)
            {
                storages[i] = ItemStorages[i].Copy();
            }
            return new MachineDialog(_states, _gameData, storages, StoragesPositionsInDialog);
        }
        public void SetPosInWindow(int x, int y, Zoom zoom)
        {
            base.SetPosInWindow(x, y);
            for (int i = 0; i < ItemStorages.Length; i++)
            {
                ItemStorages[i].SetPosInWindow((int)(x+StoragesPositionsInDialog[i].X*zoom.FlippedZoomed), (int)(y+StoragesPositionsInDialog[i].Y*zoom.FlippedZoomed));
            }
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom)
        {
            base.ActualizeDisplayingCords(newX, newY);
            for (int i = 0; i < ItemStorages.Length; i++)
            {
                ItemStorages[i].ActualizeDisplayingCords(newX+StoragesPositionsInDialog[i].X*zoom.FlippedZoomed, newY+StoragesPositionsInDialog[i].Y*zoom.FlippedZoomed, zoom);
            }
        }
    }
}