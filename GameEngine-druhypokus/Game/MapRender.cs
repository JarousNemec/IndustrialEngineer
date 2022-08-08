namespace GameEngine_druhypokus
{
    public class MapRender
    {
        public int[] GetCurrentChunks(int[] map, int mapSize, int renderArea)
        {
            if (mapSize < renderArea)
                return map;
            int[] currentChunks = new int[renderArea*renderArea];
            
            for (int y = 0; y < renderArea; y++)
            {
                for (int x = 0; x < renderArea; x++)
                {
                    currentChunks[x + y * renderArea] = map[x + y * mapSize];
                }
            }

            return currentChunks;
        }
    }
}