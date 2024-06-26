using net.roomful.api.appMenu;
using net.roomful.api.ui;

namespace net.roomful.props.editing
{
    public class SidebarTab
    {
        private readonly string m_name;
        private readonly ISidePanelController m_panel;

        public SidebarTab(string name, ISidePanelController panel) {
            m_name = name;
            m_panel = panel;
        }

        public IButtonView AddButton(ButtonData data, params ButtonOption[] options) {
            return m_panel.AddButton(m_name, data, options);
        }
    }
}
