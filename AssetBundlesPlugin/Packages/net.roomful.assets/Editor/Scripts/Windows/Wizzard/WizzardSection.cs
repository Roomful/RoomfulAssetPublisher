using UnityEngine;

namespace net.roomful.assets.Editor
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
            GUILayout.Label(m_content.text, WizardWindow.Constants.sectionHeader);

            m_panel.OnGUI();
        }


        public GUIContent Content => m_content;

        public bool CanBeSelected => m_panel.CanBeSelected;
    }
}