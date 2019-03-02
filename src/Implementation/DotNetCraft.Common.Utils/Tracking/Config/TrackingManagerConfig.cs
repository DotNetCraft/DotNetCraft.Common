namespace DotNetCraft.Common.Utils.Tracking.Config
{
    public class TrackingManagerConfig
    {
        public int MaxSize { get; set; }
        public bool IsActive { get; set; }

        public TrackingManagerConfig()
        {
            MaxSize = 10;
            IsActive = true;
        }
    }
}
