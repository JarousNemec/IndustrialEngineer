using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using IndustrialEngineer.Enums;
using IndustrialEnginner.Components;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Interfaces;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class GuiComponent : IGuiComponent
    {
        public Sprite Sprite { get; set; }
        public float DisplayingX { get; set; }
        public float DisplayingY { get; set; }
        public int GridPositionX { get; set; }
        public int GridPositionY { get; set; }
        public int ComponentPosInWindowX { get; set; }
        public int ComponentPosInWindowY { get; set; }
        public List<GuiComponent> _childComponentsToDraw;
        public ComponentType Type { get; set; }

        public GuiComponent(Sprite sprite, ComponentType type)
        {
            Type = type;
            _childComponentsToDraw = new List<GuiComponent>();
            Sprite = sprite;
        }

        public void SetPosInWindow(int x, int y)
        {
            ComponentPosInWindowX = x;
            ComponentPosInWindowY = y;
        }

        public virtual void ActualizeDisplayingCords(float newX, float newY)
        {
            DisplayingX = newX;
            DisplayingY = newY;
        }

        public virtual void Draw(RenderWindow window, Zoom zoom)
        {
            Sprite.Position = new Vector2f(DisplayingX, DisplayingY);
            Sprite.Scale = new Vector2f(zoom.FlippedZoomed, zoom.FlippedZoomed);
            window.Draw(Sprite);
            foreach (var childComponent in _childComponentsToDraw)
            {
                childComponent.Draw(window, zoom);
            }
        }
    }
}