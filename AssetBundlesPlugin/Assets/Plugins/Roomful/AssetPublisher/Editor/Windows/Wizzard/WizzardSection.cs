using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace RF.AssetWizzard.Editor
{
    public class WizzardSection
    {
        private readonly GUIContent m_content;
        private readonly IPanel m_panel;


        public WizzardSection(string name, IPanel panel) {
            m_content = new GUIContent(name);
            m_panel = panel;
        }

        public WizzardSection(string name, Texture2D icon, IPanel panel) {
            m_content = new GUIContent(name, icon);
            m_panel = panel;
        }


        public void Draw() {
            GUILayout.Label(m_content.text, WizardWindow.Constants.sectionHeader, new GUILayoutOption[0]);

            m_panel.OnGUI();
        }


        public GUIContent Content {
            get {
                return m_content;
            }
        }

        public bool CanBeSelected {
            get {
                return m_panel.CanBeSelected;
            }
        }

    }
}