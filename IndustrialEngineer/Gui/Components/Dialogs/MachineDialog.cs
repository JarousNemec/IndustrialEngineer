using System;
using System.Collections.Generic;
using IndustrialEngineer.Enums;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.GameEntities;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    [Serializable]
    public class MachineDialog : GuiComponent
    {
        public Sprite[] States { get; set; }
        public ItemStorage[] ItemStorages { get; set; }
        public ProgressBarComponent ProgressBarComponent { get; set; }
        public Vector2i[] StoragesPositionsInDialog { get; set; }

        public Vector2i ProgressBarPositionInDialog { get; set; }

        public MachineDialog(Sprite[] states, ItemStorage[] itemStorages, Vector2i[] storagesPositionsInDialog, ProgressBarComponent progressBar, Vector2i progressBarPositionInDialog) : base(states[0],
            ComponentType.Interface)
        {
            States = states;
            ItemStorages = itemStorages;
            ProgressBarComponent = progressBar;
            StoragesPositionsInDialog = storagesPositionsInDialog;
            ProgressBarPositionInDialog = progressBarPositionInDialog;
            foreach (var itemStorage in itemStorages)
            {
                _childComponentsToDraw.Add(itemStorage);
            }
            _childComponentsToDraw.Add(progressBar);
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
            return new MachineDialog(States, storages, StoragesPositionsInDialog, new ProgressBarComponent(ProgressBarComponent.Sprite, ProgressBarComponent.States), ProgressBarPositionInDialog);
        }
        public void SetPosInWindow(int x, int y, Zoom zoom)
        {
            base.SetPosInWindow(x, y);
            for (int i = 0; i < ItemStorages.Length; i++)
            {
                ItemStorages[i].SetPosInWindow((int)(x+StoragesPositionsInDialog[i].X*zoom.FlippedZoomed), (int)(y+StoragesPositionsInDialog[i].Y*zoom.FlippedZoomed));
            }
            ProgressBarComponent.SetPosInWindow((int)(x+ProgressBarPositionInDialog.X*zoom.FlippedZoomed), (int)(y+ProgressBarPositionInDialog.Y*zoom.FlippedZoomed));
        }

        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom)
        {
            base.ActualizeDisplayingCords(newX, newY);
            for (int i = 0; i < ItemStorages.Length; i++)
            {
                ItemStorages[i].ActualizeDisplayingCords(newX+StoragesPositionsInDialog[i].X*zoom.FlippedZoomed, newY+StoragesPositionsInDialog[i].Y*zoom.FlippedZoomed, zoom);
            }
            ProgressBarComponent.ActualizeDisplayingCords(newX+ProgressBarPositionInDialog.X*zoom.FlippedZoomed, newY+ProgressBarPositionInDialog.Y*zoom.FlippedZoomed);
        }
    }
}