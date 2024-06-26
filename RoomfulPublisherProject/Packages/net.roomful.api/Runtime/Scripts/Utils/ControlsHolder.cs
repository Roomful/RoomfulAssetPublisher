using System;
using System.Collections.Generic;
using net.roomful.api.settings;
using UnityEngine;

namespace net.roomful.api
{
    public class TabbedControlsHolder
    {
        private readonly Dictionary<string, ControlsContainer> m_registeredControlsByTabs = new Dictionary<string, ControlsContainer>();
        private readonly Dictionary<string, (string label, int priority)> m_registeredTab = new Dictionary<string, (string, int)>();
        private readonly Dictionary<string, IPageTemplate> m_pages = new Dictionary<string, IPageTemplate>();

        private class ControlsContainer
        {
            private readonly Dictionary<string, IControlTemplate> m_registeredControls = new Dictionary<string, IControlTemplate>();

            public void AddControl(IControlTemplate controlTemplate) {
                m_registeredControls.Add(controlTemplate.Title, controlTemplate);
            }

            public void RemoveControl(string title) {
                m_registeredControls.Remove(title);
            }

            public bool IsEmpty => m_registeredControls.Count == 0;

            public IReadOnlyDictionary<string, IControlTemplate> Controls => m_registeredControls;
        }

        public void AddControl(string tabName, Func<IControlTemplate> templateCreation) {
            tabName = EncodeTabName(tabName);
            if (m_registeredControlsByTabs.TryGetValue(tabName, out var container) == false) {
                container = new ControlsContainer();
                m_registeredControlsByTabs[tabName] = container;
            }

            container.AddControl(templateCreation.Invoke());
        }
        
        private string EncodeTabName(string tabName) {
            return tabName.ToLower().Replace(" ", "_");
        }

        public void SetPage(string tabName, Action<Action<GameObject>> onPageOpen) {
            tabName = EncodeTabName(tabName);
            m_pages[tabName] = new PageTemplate(onPageOpen);
        }

        public bool HasPage(string tabName) {
            tabName = EncodeTabName(tabName);
            return m_pages.ContainsKey(tabName);
        }

        public void CreatePage(string tabName, Action<GameObject> OnPageCreated) {
            tabName = EncodeTabName(tabName);
            m_pages[tabName].Create(OnPageCreated);
        }

        public void CreateControls(ISettingsController controller) {
            foreach (var tab in m_registeredTab) {
                controller.AddTab(tab.Key, tab.Value.label, tab.Value.priority);
            }
            
            foreach (var containerPair in m_registeredControlsByTabs) {
                foreach (var controlPair in containerPair.Value.Controls) {
                    controller.AddControl(containerPair.Key, controlPair.Value);
                }
            }
        }

        public void RemoveControl(string tabName, string title) {
            tabName = EncodeTabName(tabName);
            if (m_registeredControlsByTabs.TryGetValue(tabName, out var container)) {
                container.RemoveControl(title);
                // Remove empty tab
                if (container.IsEmpty) {
                    m_registeredControlsByTabs.Remove(tabName);
                }
            }
        }

        public bool IsExistTab(string tabName) {
            tabName = EncodeTabName(tabName);
            return m_registeredTab.ContainsKey(tabName);
        }

        public void RegisterTab(string tabName, string label, int priority) {
            tabName = EncodeTabName(tabName);
            if (!m_registeredTab.ContainsKey(tabName)) {
                m_registeredTab.Add(tabName, (label, priority));
            } 
            else {
                Debug.LogWarning($"{tabName} - such a tab exists");
            }
        }
    }
}
