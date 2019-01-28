#if UNITY_2018_3_OR_NEWER

using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor
{
    public abstract class BaseWizardTab : VisualElement
    {

        public abstract string Name { get; }


        public BaseWizardTab() {
            style.flexGrow = 0.7f;

            var label = new Label();
            label.style.fontSize = 20;
            label.style.height = 25;
            label.text = Name;
            Add(label);
        }

    }
}
#endif