
#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.UIElements.StyleEnums;

namespace RF.AssetWizzard.Editor
{
    public class AssetListView : VisualElement
    {

        private ListView m_listView;
        private List<PropTemplate> m_items = new List<PropTemplate>();
        private Action<PropTemplate> m_onItemSelect = delegate { };


        public AssetListView():base() {
            style.flexGrow = 0.3f;
            style.minWidth = 200;

            m_listView = MakeListView();
            Add(m_listView);
        }

        public void AddAsset(PropTemplate item) {
            Debug.Log("Add asset " + item.Title);
            m_items.Add(item);
            m_listView.Refresh();
        }

        public void OnTabSelectCallback(Action<PropTemplate> callback) {
            m_onItemSelect = callback;
        }


        public PropTemplate Selected {
            get {
                return m_items[m_listView.selectedIndex];
            }
        }

        public int ItemsCount { get { return m_items.Count;} }

        private ListView MakeListView() {
            Func<VisualElement> makeItem = () => {
                var box = new VisualElement();
                box.style.flexDirection = FlexDirection.Row;
                box.style.flexGrow = 1f;
                var label = new Label();

                label.style.minHeight = 50;
                label.style.minWidth = 100;
                label.style.marginTop = 1;
                label.style.marginBottom = 0;

                label.style.paddingTop = 0;
                label.style.paddingBottom = 0;

                box.Add(label);
                return box;
            };

            Action<VisualElement, int> bindItem = (VisualElement e, int index) => {
                (e.ElementAt(0) as Label).text = m_items[index].Title;
            };
            
            var inlineListView = new ListView(m_items, (int) EditorGUIUtility.singleLineHeight, makeItem, bindItem);
            InitListView(inlineListView, "Assets_List_View");
            return inlineListView;
        }

        internal void UpdateItem(string assetId, ReleaseStatus newReleaseStatus) {
            var template = m_items.Find(item => item.Id.Equals(assetId));
            if (template != null) {
                template.ReleaseStatus = newReleaseStatus;
                if (m_items.IndexOf(template) == m_listView.selectedIndex) {
                    m_onItemSelect.Invoke(template);
                }
                m_listView.Refresh();
            }
        }

        internal void RemoveItem(string assetId) {
            var template = m_items.Find(item => item.Id.Equals(assetId));
            if (template != null) {
                m_items.Remove(template);
                m_listView.Refresh();
            }
        }

        private void InitListView(ListView listView, string name) {
            listView.persistenceKey = name;
            listView.selectionType = SelectionType.Single;

            listView.onItemChosen += (obj) => {
               Debug.Log("onItemChosen: " + obj);
            };


            listView.onSelectionChanged += (objects) => {
                m_onItemSelect.Invoke((PropTemplate)objects[0]);
            };

            listView.style.flexGrow = 1f;
            listView.style.flexShrink = 0f;
            listView.style.flexBasis = 0f;
        }



    }
}

#endif