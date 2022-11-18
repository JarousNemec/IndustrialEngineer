using System.Collections.Generic;

namespace IndustrialEnginner.Blocks
{
    public class BlockRegistry
    {
        public Grass Grass { get; set; }

        public Water Water { get; set; }

        public Tree Tree { get; set; }

        public Stone Stone { get; set; }

        public Copper Copper { get; set; }

        public Diamond Diamond { get; set; }

        public Gold Gold { get; set; }

        public Emerald Emerald { get; set; }

        public Sand Sand { get; set; }

        public Workbench Workbench { get; set; }

        public Bridge Bridge { get; set; }

        public Coal Coal { get; set; }
        // Grass = 0, Water = 1, Tree = 2, Stone = 3, Copper = 4, Diamond = 5, Gold = 6, Emerald = 7, Sand = 8, Workbench = 9, BridgeSt1 = 10, Coal = 11, BridgeSt2 = 12, BridgeSt3 = 13

        public List<Block> Registry { get; set; }

        public BlockRegistry()
        {
            Registry = new List<Block>();
        }
    }
}