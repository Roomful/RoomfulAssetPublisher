using System;
using TMPro;
using UnityEngine;

namespace net.roomful.api
{
    public class EditFieldControlTemplate : DefaultControlTemplate<string>
    {
        private TMP_InputField m_field;
        private readonly Func<string, string> m_validator;
        private string m_prevValue;

        public EditFieldControlTemplate(string title, Action<string> valueChangedDelegate,
            Func<string> initValueDelegate,
            Func<GameObject> createPrefabDelegate, Func<string, string> validator)
            : base(title, valueChangedDelegate, initValueDelegate, createPrefabDelegate)
        {
            m_validator = validator;
        }

        public override void DisableInteraction() {
            m_field.interactable = false;
        }

        protected override void OnControlInstantiated(GameObject control) {
            base.OnControlInstantiated(control);

            m_field = control.GetComponentInChildren<TMP_InputField>();
            m_field.onEndEdit.AddListener(OnEndEdit);
        }

        private void OnEndEdit(string value)
        {
            string result = m_validator(value);
            if (result == null) {
                m_field.text = m_prevValue;
            } 
            else if (m_prevValue != result) {
                ValueChangedDelegate.Invoke(result);
                m_prevValue = result;
                m_field.text = result;
            }
        }

        protected override void SetInitialValue(string initValue)
        {
            m_prevValue = initValue;
            m_field.text = m_prevValue;
        }
    }
}
