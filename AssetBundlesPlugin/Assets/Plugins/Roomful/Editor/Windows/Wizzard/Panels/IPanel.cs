namespace RF.AssetWizzard.Editor
{

    public interface IPanel
    {

        void OnGUI();
        bool CanBeSelected { get;  }

    }
}