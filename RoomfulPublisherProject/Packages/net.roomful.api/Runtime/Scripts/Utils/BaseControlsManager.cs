using System;
using System.Collections.Generic;
using net.roomful.api;
using net.roomful.api.settings;
using UnityEngine;

namespace net.roomful.settingsModule
{
    public abstract class BaseControlsManager<T>
    {
        protected readonly TabbedControlsHolder m_Controls = new TabbedControlsHolder();
        protected IRoomfulSettingsUIService m_SettingsUIService;
        
        
        bool m_IsInitialized;
        Action m_OnInit;
        readonly Stack<IBuildSettingsViewDelegate<T>> m_BuildViewDelegates = new Stack<IBuildSettingsViewDelegate<T>>();

        protected abstract T ServiceInstance { get; }

        public void RegisterBuildViewDelegate(IBuildSettingsViewDelegate<T> buildViewDelegate) {
            m_BuildViewDelegates.Push(buildViewDelegate);
        }

        protected void Init(Action onInit)
        {
            if (m_IsInitialized) {
                onInit.Invoke();
                return;
            }
            
            m_OnInit = onInit;
            OnInit(() =>
            {
                m_IsInitialized = true;
                BuildDefaultView();
                BuildNextCustomView();
            });
        }

        protected abstract void OnInit(Action onInit);
        protected abstract void BuildDefaultView();

        protected void BuildNextCustomView() {
            if (m_BuildViewDelegates.Count == 0) {
                m_OnInit.Invoke();
                return;
            }

            var customViewBuilder = m_BuildViewDelegates.Pop();
            customViewBuilder.BuildView(ServiceInstance, BuildNextCustomView);
        } 

        public void AddTab(string tabName, string label, int priority) {
            m_Controls.RegisterTab(tabName, label, priority);
        }

        public void AddTooltip(string tabName, string controlName, string tooltip) {
            
        }
        
        public void SetPage(string tabName, Action<Action<GameObject>> onPageOpen) {
            m_Controls.SetPage(tabName, onPageOpen);
        }

        public void AddCheckbox(string tabName, string title, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            m_Controls.AddControl(tabName, ()=> ControlsUtility.CreateCheckboxTemplate(title, valueChangedDelegate, getValueDelegate));
        }

        public void AddToggle(string tabName, string title, string leftLabel, string rightLabel, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            m_Controls.AddControl(tabName, ()=> ControlsUtility.CreateToggleTemplate(title, leftLabel, rightLabel, valueChangedDelegate, getValueDelegate));
        }

        public void AddSlider(string tabName, string title, Action<float> valueChangedDelegate, Func<float> getValueDelegate, (float minValue, float maxValue) range) {
            m_Controls.AddControl(tabName, ()=> ControlsUtility.CreateSliderTemplate(title, valueChangedDelegate, getValueDelegate, range));
        }

        public void AddDropdown(string tabName, string title, Action<int> valueChangedDelegate, Func<int> getValueDelegate, List<string> options) {
            m_Controls.AddControl(tabName, ()=> ControlsUtility.CreateDropdownTemplate(title, valueChangedDelegate, getValueDelegate, options));
        }

        public void AddControl(string tabName, string title, string controlID) {
            m_Controls.AddControl(tabName, () => ControlsUtility.CreateControl(title, controlID));
        }

        public void AddEditField(string tabName, string title, Action<string> valueChangedDelegate, Func<string> getValueDelegate, Func<string, string> validator)
        {
            m_Controls.AddControl(tabName, ()=> ControlsUtility.CreateEditFieldTemplate(title, valueChangedDelegate, getValueDelegate, validator));
        }

        public void RemoveControl(string tabName, string title) {
            m_Controls.RemoveControl(tabName, title);
        }

        public bool IsExistTab(string tabName) {
            return m_Controls.IsExistTab(tabName);
        }

        public void Register(IRoomfulSettingsUIService uiService) {
            m_SettingsUIService = uiService;
        }
    }
}

