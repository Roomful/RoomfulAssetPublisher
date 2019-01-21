
#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor
{
    public class SidebarMenu : VisualElement
    {

        private ListView m_listView;
        private List<BaseWizzardTab> m_items = new List<BaseWizzardTab>();
        private Action<BaseWizzardTab> m_onItemSelect = delegate { };


        public SidebarMenu():base() {
            style.flexGrow = 0.3f;
            style.minWidth = 200;


            m_listView = MakeListView();
            Add(m_listView);
        }

        public void AddTabItem(BaseWizzardTab item) {
            m_items.Add(item);
            m_listView.selectedIndex = 0;
        }

        public void AddOnTabSelectCallback(Action<BaseWizzardTab> callback) {
            m_onItemSelect += callback;
        }


        public BaseWizzardTab Selected {
            get {
                return m_items[m_listView.selectedIndex];
            }
        }

        private ListView MakeListView() {
            Func<VisualElement> makeItem = () => {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.flexGrow = 1f;
                var label = new Label();

                label.style.marginTop = 1;
                label.style.marginBottom = 0;

                label.style.paddingTop = 0;
                label.style.paddingBottom = 0;

                box.Add(label);
                return box;
            };

            Action<VisualElement, int> bindItem = (VisualElement e, int index) => {
                (e.ElementAt(0) as Label).text = m_items[index].Name;
            };
            
            var inlineListView = new ListView(m_items, (int) EditorGUIUtility.singleLineHeight, makeItem, bindItem);
            InitListView(inlineListView, "Inline_View");
            return inlineListView;
        }





        private void InitListView(ListView listView, string name) {
            listView.persistenceKey = name;
            listView.selectionType = SelectionType.Multiple;

            listView.onItemChosen += (obj) => {
               // Debug.Log("onItemChosen: " + obj);
            };


            listView.onSelectionChanged += (objects) => {
                m_onItemSelect.Invoke((BaseWizzardTab)objects[0]);
            };

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 0f;
            listView.style.flexBasis = 0f;
        }



    }
}

#endif