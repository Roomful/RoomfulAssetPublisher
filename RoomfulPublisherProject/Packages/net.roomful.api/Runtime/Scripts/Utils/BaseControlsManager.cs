using System;
using System.Collections.Generic;
using net.roomful.api;
using net.roomful.api.settings;

namespace net.roomful.settingsModule
{
    public abstract class BaseControlsManager
    {
        protected TabbedControlsHolder m_controls = new TabbedControlsHolder();
        protected IRoomfulSettingsUIService m_settingsUIService;

        public void AddTab(string tabName, string label, int priority) {
            m_controls.RegisterTab(tabName, label, priority);
        }

        public void AddCheckbox(string tabName, string title, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            m_controls.AddControl(tabName, ()=> ControlsUtility.CreateCheckboxTemplate(title, valueChangedDelegate, getValueDelegate));
        }

        public void AddToggle(string tabName, string title, string leftLabel, string rightLabel, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            m_controls.AddControl(tabName, ()=> ControlsUtility.CreateToggleTemplate(title, leftLabel, rightLabel, valueChangedDelegate, getValueDelegate));
        }

        public void AddSlider(string tabName, string title, Action<float> valueChangedDelegate, Func<float> getValueDelegate, (float minValue, float maxValue) range) {
            m_controls.AddControl(tabName, ()=> ControlsUtility.CreateSliderTemplate(title, valueChangedDelegate, getValueDelegate, range));
        }

        public void AddDropdown(string tabName, string title, Action<int> valueChangedDelegate, Func<int> getValueDelegate, List<string> options) {
            m_controls.AddControl(tabName, ()=> ControlsUtility.CreateDropdownTemplate(title, valueChangedDelegate, getValueDelegate, options));
        }

        public void AddControl(string tabName, string title, string controlID) {
            m_controls.AddControl(tabName, () => ControlsUtility.CreateControl(title, controlID));
        }

        public void AddEditField(string tabName, string title, Action<string> valueChangedDelegate, Func<string> getValueDelegate, Func<string, string> validator)
        {
            m_controls.AddControl(tabName, ()=> ControlsUtility.CreateEditFieldTemplate(title, valueChangedDelegate, getValueDelegate, validator));
        }

        public void RemoveControl(string tabName, string title) {
            m_controls.RemoveControl(tabName, title);
        }

        public bool IsExistTab(string tabName) {
            return m_controls.IsExistTab(tabName);
        }

        public void Register(IRoomfulSettingsUIService uiService) {
            m_settingsUIService = uiService;
        }
    }
}

