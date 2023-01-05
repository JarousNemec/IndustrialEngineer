using System;
using System.Collections.Generic;
using System.Diagnostics;
using IndustrialEnginner.GameEntities;
using IndustrialEnginner.Interfaces;

namespace IndustrialEnginner
{
    public class WorldUpdater : IThread
    {
        public const int TARGET_FPS = 60;
        public const long MINIMAL_TIME_BETWEEN_FRAMES = TimeSpan.TicksPerSecond / TARGET_FPS;
        public GameTime UpdaterTime { get; protected set; }
        public World World { get; set; }
        
        public List<Building> RenderedEntities { get; set; }
        public WorldUpdater(World world)
        {
            UpdaterTime = new GameTime();
            World = world;
        }
        
        public void Run()
        {
            long actualTime;
            long previousTime = 0;
            long deltaTime;
            long totalDeltaTime = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                actualTime =  stopwatch.ElapsedTicks;
                deltaTime = actualTime - previousTime;
                previousTime = actualTime;
                totalDeltaTime += deltaTime;
                

                if (totalDeltaTime < MINIMAL_TIME_BETWEEN_FRAMES) continue;
                UpdaterTime.Update(SecondsFromTicks(totalDeltaTime), SecondsFromTicks(stopwatch.ElapsedTicks));
                totalDeltaTime = 0;
                Update();
            }
        }
        private float SecondsFromTicks(long ticks)
        {
            return (float)ticks / TimeSpan.TicksPerSecond;
        }
        private void Update()
        {
            UpdateEntities(UpdaterTime.DeltaTime);
        }
        
        private void UpdateEntities(float deltaTime)
        {
            for (int i = RenderedEntities.Count; i > 0; i--)
            {
                RenderedEntities[i-1].Update(deltaTime, World);
            }
        }
    }
}