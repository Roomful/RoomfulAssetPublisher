#if UNITY_2018_3_OR_NEWER

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
        private List<BaseWizardTab> m_items = new List<BaseWizardTab>();
        private Action<BaseWizardTab> m_onItemSelect = delegate {};

        public SidebarMenu() {
            style.flexGrow = 0.3f;
            style.minWidth = 200;
            m_listView = MakeListView();
            Add(m_listView);
        }

        public void AddTabItem(BaseWizardTab item) {
            m_items.Add(item);
            m_listView.selectedIndex = 0;
        }

        public void AddOnTabSelectCallback(Action<BaseWizardTab> callback) {
            m_onItemSelect += callback;
        }

        public BaseWizardTab Selected => m_items[m_listView.selectedIndex];

        private ListView MakeListView() {
            Func<VisualElement> makeItem = () => {
                var box = new VisualElement {
                    new Label {
                        style = {
                            marginTop = 1,
                            marginBottom = 0,
                            paddingTop = 0,
                            paddingBottom = 0
                        }
                    }
                };
                box.style.flexDirection = FlexDirection.Row;
                box.style.flexGrow = 1f;
                
                return box;
            };
            Action<VisualElement, int> bindItem = (e, index) => {
                (e.ElementAt(0) as Label).text = m_items[index].Name;
            };
            var inlineListView = new ListView(m_items, (int) EditorGUIUtility.singleLineHeight, makeItem, bindItem);
            InitListView(inlineListView, "Inline_View");
            return inlineListView;
        }

        private void InitListView(ListView listView, string name) {
            listView.persistenceKey = name;
            listView.selectionType = SelectionType.Multiple;
            listView.onSelectionChanged += objects => {
                m_onItemSelect.Invoke((BaseWizardTab)objects[0]);
            };
            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 0f;
            listView.style.flexBasis = 0f;
        }
    }
}

#endif