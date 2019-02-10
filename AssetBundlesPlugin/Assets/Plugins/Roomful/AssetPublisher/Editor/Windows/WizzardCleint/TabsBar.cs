#if UNITY_2018_3_OR_NEWER

using UnityEngine.Experimental.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor {
    
    public class TabsBar : VisualContainer {
        
        private readonly Dictionary<string, TabBarButton> m_tabs = new Dictionary<string, TabBarButton>();
        private string m_selectedTab;
        
        public TabsBar() {
            style.minWidth = 150;
            style.maxWidth = 150;
            style.height = 25;
            style.flexDirection = FlexDirection.Row;
        }
        
        public void AddTabItem(TabBarButton button) {
            m_tabs.Add(button.Name, button);
        }

        public void Init() {
            foreach (var tab in m_tabs) {
                Add(tab.Value);
                tab.Value.AddListenerForClick(name => {
                    m_selectedTab = name;
                    SetTabsSelection();
                });
            }

            SelectFirstElementByDefault();
        }

        private void SelectFirstElementByDefault() {
            m_selectedTab = m_tabs.First().Key;
            SetTabsSelection();
        }

        private void SetTabsSelection() {
            foreach(var tab in m_tabs.Keys) {
                if (!tab.Equals (m_selectedTab)) {
                    m_tabs [tab].Hide ();
                }
            }
            m_tabs [m_selectedTab].Show ();
        }
    }
}
#endif
