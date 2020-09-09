namespace net.roomful.assets.Editor
{
    internal interface IPanel
    {
        void OnGUI();
        bool CanBeSelected { get; }
    }
}