using System;
using UnityEngine;
using UnityEngine.UI;

namespace net.roomful.api
{
    public class SliderControlTemplate : DefaultControlTemplate<float>
    {
        private readonly float m_minValue;
        private readonly float m_maxValue;
        private Slider m_slider;

        public SliderControlTemplate(string title, Action<float> valueChangedDelegate, Func<float> initValueDelegate,
            (float minValue, float maxValue) range, Func<GameObject> createPrefabDelegate) : base(title, valueChangedDelegate, initValueDelegate, createPrefabDelegate) {

            m_minValue = range.minValue;
            m_maxValue = range.maxValue;
        }

        public override void DisableInteraction() {
            m_slider.interactable = false;
        }

        protected override void OnControlInstantiated(GameObject control) {
            base.OnControlInstantiated(control);

            m_slider = control.GetComponentInChildren<Slider>();

            m_slider.onValueChanged.AddListener((val=> {
                ValueChangedDelegate.Invoke(val);
            }));
        }

        protected override void SetInitialValue(float initValue) {
            m_slider.minValue = m_minValue;
            m_slider.maxValue = m_maxValue;
            m_slider.SetValueWithoutNotify(initValue);
        }
    }
}