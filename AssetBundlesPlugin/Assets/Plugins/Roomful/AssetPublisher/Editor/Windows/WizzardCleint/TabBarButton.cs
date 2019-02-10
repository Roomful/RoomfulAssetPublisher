using System;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor {

    public abstract class TabBarButton : VisualElement, ITabBarButton {

        protected Button Button { get; }
        protected Action<string> m_onClick = delegate { };

        protected TabBarButton() {
            Button = new Button(ButtonClickHandler) {
                style = {
                    height = 20,
                    width = 90
                }
            };
            Add(Button);
        }

        protected abstract void ButtonClickHandler();

        public virtual string Name { get; }
        public virtual void Show() {}
        public virtual void Hide() {}
        public virtual void AddListenerForClick(Action<string> callback) {}
    }
}