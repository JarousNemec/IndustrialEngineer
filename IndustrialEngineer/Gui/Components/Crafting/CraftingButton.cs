using IndustrialEngineer.Enums;
using IndustrialEnginner.CraftingRecipies;
using IndustrialEnginner.DataModels;
using IndustrialEnginner.Items;
using SFML.Graphics;
using SFML.System;

namespace IndustrialEnginner.Gui
{
    public class CraftingButton : GuiComponent
    {
        public Recipe Recipe { get; set; }
        private readonly PictureBox _recipeIcon;
        public CraftingButton(Sprite sprite,Recipe recipe, ComponentType type) : base(sprite,
            type)
        {
            Recipe = recipe;
            _recipeIcon = new PictureBox(recipe.Sprite);
            _childComponentsToDraw.Add(_recipeIcon);
        }
        public void ActualizeDisplayingCords(float newX, float newY, Zoom zoom, Vector2u slotSize)
        {
            base.ActualizeDisplayingCords(newX, newY);

            var picturePosX = CalculatePicturePosition(newX, newY, zoom, out var picturePosY);
            _recipeIcon.ActualizeDisplayingCords(picturePosX, picturePosY);
        }

        private static float CalculatePicturePosition(float newX, float newY, Zoom zoom, out float picturePosY)
        {
            var picturePosX = newX + 6 * zoom.FlippedZoomed;
            picturePosY = newY + 6 * zoom.FlippedZoomed;
            return picturePosX;
        }
    }
}