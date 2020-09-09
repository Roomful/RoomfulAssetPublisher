using UnityEngine;

namespace net.roomful.assets.Editor
{
    internal class WizzardSection
    {
        private readonly IPanel m_panel;

        public WizzardSection(string name, IPanel panel) {
            Content = new GUIContent(name);
            m_panel = panel;
        }

        public void Draw() {
            GUILayout.Label(Content.text, WizardWindow.Constants.sectionHeader);

            m_panel.OnGUI();
        }

        public GUIContent Content { get; }

        public bool CanBeSelected => m_panel.CanBeSelected;
    }
}