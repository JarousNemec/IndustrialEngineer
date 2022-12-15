using System.Collections.Generic;
using SFML.Graphics;

namespace IndustrialEnginner.GameEntities
{
    public class GraphicsEntity
    {
        public GraphicsEntityProperties Properties { get; set; }

        public GraphicsEntity(GraphicsEntityProperties properties)
        {
            Properties = properties;
        }

        public GraphicsEntity Copy()
        {
            return new GraphicsEntity(Properties.Copy());
        }
    }
}