using UnityEngine;

namespace net.roomful.assets.editor
{
    class WizardSection
    {
        readonly IPanel m_Panel;

        public WizardSection(string name, IPanel panel) {
            Content = new GUIContent(name);
            m_Panel = panel;
        }

        public void Draw()
        {
            GUILayout.Label(Content.text, WizardWindow.Constants.sectionHeader);
            m_Panel.OnGUI();
        }

        public GUIContent Content { get; }

        public bool CanBeSelected => m_Panel.CanBeSelected;
    }
}