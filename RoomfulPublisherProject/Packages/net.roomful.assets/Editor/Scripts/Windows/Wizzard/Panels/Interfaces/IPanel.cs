namespace net.roomful.assets.editor
{
    interface IPanel
    {
        void OnGUI();
        bool CanBeSelected { get; }
    }
}