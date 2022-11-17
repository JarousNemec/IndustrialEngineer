namespace IndustrialEnginner.DataModels
{
    public class Zoom
    {
        public uint ZoomStep { get; set; }//2
        public float Zoomed { get; set; } //= 1;
        public float FlippedZoomed { get; set; } //= 2;
        public float MaxZoom { get; set; } //= 4;
        public float MinZoom { get; set; } //= 0.5f;

        public Zoom(uint zoomStep, float zoomed, float flippedZoomed, float maxZoom, float minZoom)
        {
            ZoomStep = zoomStep;
            Zoomed = zoomed;
            FlippedZoomed = flippedZoomed;
            MaxZoom = maxZoom;
            MinZoom = minZoom;
        }
    }
}