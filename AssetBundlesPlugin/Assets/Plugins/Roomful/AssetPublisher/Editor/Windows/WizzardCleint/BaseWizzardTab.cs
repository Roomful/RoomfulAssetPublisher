#if UNITY_2018_3_OR_NEWER

using System;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor
{
    [Serializable]
    public abstract class BaseWizardTab : VisualElement
    {
        public abstract string Name { get; }

        protected BaseWizardTab() {
            style.flexGrow = 0.7f;

            var label = new Label {
                text = Name,
                style = {
                    fontSize = 20,
                    height = 25
                }
            };
            Add(label);
        }
    }
}
#endif