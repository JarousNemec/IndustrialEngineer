using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class Furnace : Building
    {
        public Furnace(BuildingProperties properties) : base(properties)
        {
        }

        public override void Update(float deltaTime, World world, GameData gameData)
        {
            base.Update(deltaTime, world, gameData);
        }
    }
}