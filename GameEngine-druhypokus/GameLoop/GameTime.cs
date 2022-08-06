using System.Security.Cryptography.X509Certificates;

namespace GameEngine_druhypokus
{
    public class GameTime
    {
        private float deltaTime = 0f;
        private float timeScale = 1f;

        public float TimeScale
        {
            get => timeScale;
            set => timeScale = value;
        }

        public float DeltaTime
        {
            get => deltaTime * timeScale;
            set => deltaTime = value;
        }

        public float DeltaTimeUnscaled => deltaTime;

        public float TotalTimeElapsed
        {
            get;
            private set;
        }

        public GameTime()
        {
            
        }

        public void Update(float deltaTime, float totalTimeElapsed)
        {
            this.deltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
        
    }
}