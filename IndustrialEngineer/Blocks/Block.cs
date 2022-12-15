using IndustrialEnginner.Interfaces;

namespace IndustrialEngineer.Blocks
{
    public class Block : IBlock
    {
        public BlockProperties Properties { get; set; }

        public Block(BlockProperties properties)
        {
            Properties = properties;
        }

        public Block Copy()
        {
            return new Block(Properties.Copy());
        }
    }
}