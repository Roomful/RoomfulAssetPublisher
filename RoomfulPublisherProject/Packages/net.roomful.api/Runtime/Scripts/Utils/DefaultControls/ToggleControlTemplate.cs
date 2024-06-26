using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace net.roomful.api
{
    public class ToggleControlTemplate : DefaultControlTemplate<bool>
    {
        private Toggle m_toggle;
        private string m_leftText;
        private string m_rightText;

        private TextMeshProUGUI m_leftLabel;
        private TextMeshProUGUI m_rightLabel;

        public ToggleControlTemplate(string title, string leftLabel, string rightLabel,
            Action<bool> valueChangedDelegate, Func<bool> initValueDelegate, Func<GameObject> createPrefabDelegate) 
            : base(title, valueChangedDelegate, initValueDelegate, createPrefabDelegate)
        {
            m_leftText = leftLabel;
            m_rightText = rightLabel;
        }

        public override void DisableInteraction() {
            m_toggle.interactable = false;
            m_toggle.graphic.color = m_toggle.colors.disabledColor;
        }

        protected override void OnControlInstantiated(GameObject control) {
            base.OnControlInstantiated(control);

            m_toggle = control.GetComponentInChildren<Toggle>();
            m_toggle.onValueChanged.AddListener(val=> {
                ValueChangedDelegate.Invoke(val);
                SetTextColors(val);
            });

            m_leftLabel = m_toggle.transform.GetChild(0).GetComponent<TextMeshProUGUI>(); 
            m_leftLabel.text = m_leftText;
            m_rightLabel = m_toggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            m_rightLabel.text = m_rightText;
        }

        protected override void SetInitialValue(bool initValue) {
            m_toggle.SetIsOnWithoutNotify(initValue);
            SetTextColors(initValue);
        }

        private void SetTextColors(bool value) {
            if(value)
            {
                m_leftLabel.color = new Color(0.7686275f, 0.7686275f, 0.7686275f);
                m_rightLabel.color = Color.black;
            }
            else {
                m_leftLabel.color = Color.black;
                m_rightLabel.color = new Color(0.7686275f, 0.7686275f, 0.7686275f);
            }
        }
    }
}
