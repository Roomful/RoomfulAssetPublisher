using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace net.roomful.api
{
    public class DropdownControlTemplate : DefaultControlTemplate<int>
    {
        private readonly List<string> m_sourceOptions;
        private TMP_Dropdown m_dropdown;
        private bool m_nothingSelected;

        private readonly List<string> m_dropdownOptions = new List<string>();

        public DropdownControlTemplate(string title, Action<int> valueChangedDelegate, Func<int> initValueDelegate,
            Func<GameObject> createPrefabDelegate, List<string> options) : base(title, valueChangedDelegate, initValueDelegate, createPrefabDelegate) {

            m_sourceOptions = options;
        }
        protected override void SetInitialValue(int initValue) {
            m_nothingSelected = initValue < 0;
            m_dropdown.SetValueWithoutNotify(initValue);

            UpdateOptions();
        }

        protected override void OnControlInstantiated(GameObject control) {
            base.OnControlInstantiated(control);

            m_dropdown = control.GetComponentInChildren<TMP_Dropdown>();
            m_dropdown.onValueChanged.AddListener((val) => {
                if (m_nothingSelected) {
                    if (m_nothingSelected && val > 0) {
                        m_nothingSelected = false;

                        ValueChangedDelegate.Invoke(val-1);

                        UpdateOptions();
                        m_dropdown.SetValueWithoutNotify(val-1);
                    }
                }
                else {
                    ValueChangedDelegate.Invoke(val);
                }
            });
        }

        private void UpdateOptions() {
            m_dropdownOptions.Clear();
            if (m_nothingSelected) {
                m_dropdownOptions.Add("Not selected");
            }
            m_dropdownOptions.AddRange(m_sourceOptions);

            m_dropdown.options = m_dropdownOptions.Select(o => new TMP_Dropdown.OptionData(o)).ToList();
        }
    }
}