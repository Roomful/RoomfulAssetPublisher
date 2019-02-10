using System;

namespace RF.AssetWizzard.Editor {

    public interface ITabBarButton {
        
        string Name { get; }
        void Show();
        void Hide();
        void AddListenerForClick(Action<string> callback);
    }
}