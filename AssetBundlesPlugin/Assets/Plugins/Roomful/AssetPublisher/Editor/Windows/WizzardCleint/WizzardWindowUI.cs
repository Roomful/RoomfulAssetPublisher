#if UNITY_2018_3_OR_NEWER

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;


namespace RF.AssetWizzard.Editor
{
    public class WizzardWindowUI : EditorWindow
    {

        private BaseWizardTab m_currentSelectedTab = null;
        private SidebarMenu m_sideBar = null;

        private VisualElement m_tabContainer;

        private void OnEnable() {

            
            wantsMouseMove = true;

            var root = this.GetRootVisualContainer();

            var toolbar = new Toolbar();
            toolbar.style.marginTop = -3;
            root.Add(toolbar);


            var splitter = new VisualSplitter();
            splitter.style.flexGrow = 1f;
            splitter.style.flexDirection = FlexDirection.Row;

            root.Add(splitter);


            m_sideBar = new SidebarMenu();
            m_sideBar.AddTabItem(new AccountTab());
            m_sideBar.AddTabItem(new MyPropsTab());
            m_sideBar.AddTabItem(new PendingPropsTab());
            m_sideBar.AddTabItem(new UserSettingsTab());

            m_sideBar.AddOnTabSelectCallback(OnTabSelect);

            splitter.Add(m_sideBar);

            m_tabContainer = new VisualElement();
            m_tabContainer.style.marginLeft = 10;
            m_tabContainer.style.flexGrow = 0.7f;
            splitter.Add(m_tabContainer);


            UpdateMainView();

        }

        private void OnTabSelect(BaseWizardTab tab) {
            Debug.Log("Selected: " + tab.Name);
            UpdateMainView();
        }



        private void UpdateMainView() {

            
            if(m_currentSelectedTab != null) {
                m_currentSelectedTab.RemoveFromHierarchy();
            }

            m_currentSelectedTab = m_sideBar.Selected;
            m_tabContainer.Add(m_currentSelectedTab);


        }


    }


}

#endif