namespace net.roomful.uiSynchronizer
{
    public interface IUISynchronizedSubject
    {
        void LeftMenuChanged(float padding, float spacing, float freeHeight);
        void MinimizeUI(bool isMinimize);
    }
}
