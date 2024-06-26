using System;
using UnityEngine;
using UnityEngine.UI;

namespace net.roomful.api
{
    public class CheckboxControlTemplate : DefaultControlTemplate<bool>
    {
        private Toggle m_toggle;

        public CheckboxControlTemplate(string title, Action<bool> valueChangedDelegate, Func<bool> initValueDelegate,
            Func<GameObject> createPrefabDelegate) : base(title, valueChangedDelegate, initValueDelegate, createPrefabDelegate) { }

        protected override void OnControlInstantiated(GameObject control) {
            base.OnControlInstantiated(control);

            m_toggle = control.GetComponentInChildren<Toggle>();
            m_toggle.onValueChanged.AddListener((val=> ValueChangedDelegate.Invoke(val)));
        }

        public override void DisableInteraction() {
            m_toggle.interactable = false;
            m_toggle.graphic.color = m_toggle.colors.disabledColor;
        }

        protected override void SetInitialValue(bool initValue) {
            m_toggle.SetIsOnWithoutNotify(initValue);
        }
    }
}