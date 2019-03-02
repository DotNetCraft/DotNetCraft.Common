namespace DotNetCraft.Common.Core.Utils.Tracking
{
    public interface ITrackingManager
    {
        void Attach(INotifyPropertyChangedExtended instance);
        void Detach(INotifyPropertyChangedExtended instance);
        bool Undo();
        void Clear();
    }
}
