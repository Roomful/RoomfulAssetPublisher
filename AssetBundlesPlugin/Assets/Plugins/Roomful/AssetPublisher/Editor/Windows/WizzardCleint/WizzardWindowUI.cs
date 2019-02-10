#if UNITY_2018_3_OR_NEWER

using RF.AssetWizzard.Managers;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor
{
    public class WizzardWindowUI : EditorWindow
    {
        private BaseWizardTab m_currentSelectedTab;
        private SidebarMenu m_sideBar;
        private VisualElement m_tabContainer;

        private void OnEnable() {
            wantsMouseMove = true;
            var root = this.GetRootVisualContainer();
            var toolbar = new Toolbar();
            toolbar.style.marginTop = -3;
            root.Add(toolbar);

            var splitter = new VisualSplitter {
                style = {
                    flexGrow = 1f,
                    flexDirection = FlexDirection.Row
                }
            };
            root.Add(splitter);

            m_sideBar = new SidebarMenu();
            m_sideBar.AddTabItem(new AccountTab());
            m_sideBar.AddTabItem(new MyAssetsTab());
            m_sideBar.AddTabItem(new PendingPropsTab());
            m_sideBar.AddTabItem(new UserSettingsTab());
            m_sideBar.AddOnTabSelectCallback(OnTabSelect);
            
            splitter.Add(m_sideBar);
            
            m_tabContainer = new VisualElement {
                style = {
                    marginLeft = 10,
                    flexGrow = 0.7f
                }
            };
            splitter.Add(m_tabContainer);
            UpdateMainView();
        }

        private void OnTabSelect(BaseWizardTab tab) {
            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                UpdateMainView();
            }
        }

        private void UpdateMainView() {
            m_currentSelectedTab?.RemoveFromHierarchy();
            m_currentSelectedTab = m_sideBar.Selected;
            m_tabContainer.Add(m_currentSelectedTab);
        }
        
        void OnDestroy() {
            UserManager.OnUserTemplateUpdate.RemoveAllListeners();
        }
    }
}

#endif