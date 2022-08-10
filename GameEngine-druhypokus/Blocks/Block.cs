using SFML.Graphics;

namespace GameEngine_druhypokus.GameEntities
{
    public class Block
    {
        public bool CanStepOn { get; private set; }
        public int Id { get; set; }
        

        public Block(int id, bool canStepOn = true)
        {
            Id = id;
            CanStepOn = canStepOn;
        }
    }
}