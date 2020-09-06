namespace net.roomful.assets.Editor
{

    public interface IPanel
    {

        void OnGUI();
        bool CanBeSelected { get;  }

    }
}