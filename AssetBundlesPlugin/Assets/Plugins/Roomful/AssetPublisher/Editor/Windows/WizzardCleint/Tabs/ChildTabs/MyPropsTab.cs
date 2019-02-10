using System;

namespace RF.AssetWizzard.Editor {
    
    public class MyPropsTab : TabBarButton {
        
        public override string Name => "Props";

        public MyPropsTab() {
            Button.text = Name;
        }
        
        protected override void ButtonClickHandler() {
            m_onClick.Invoke(Name);
        }

        public override void AddListenerForClick(Action<string> callback) {
            m_onClick = callback;
        }

        public override void Show() {
        }

        public override void Hide() {
            
        }
    }
}
