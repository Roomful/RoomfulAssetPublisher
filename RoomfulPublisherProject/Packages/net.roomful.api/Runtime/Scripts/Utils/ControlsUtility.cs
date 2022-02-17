using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public static class ControlsUtility
    {
        // This provider placed in RoomSettings scene under ControlsProvider GameObject
        private static IControlsProvider s_provider;

        public static void Initialize(IControlsProvider provider) {
            s_provider = provider;
        }

        public static IControlTemplate CreateCheckboxTemplate(string title, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            return new CheckboxControlTemplate(title, valueChangedDelegate, getValueDelegate, () => s_provider.GetControlPrefab("Checkbox"));
        }

        public static IControlTemplate CreateEditFieldTemplate(string title, Action<string> valueChangedDelegate, Func<string> getValueDelegate, Func<string, string> validator) {
            return new EditFieldControlTemplate(title, valueChangedDelegate, getValueDelegate, () => s_provider.GetControlPrefab("EditField"), validator);
        }

        public static IControlTemplate CreateToggleTemplate(string title, string leftLabel, string rightLabel, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate) {
            return new ToggleControlTemplate(title, leftLabel, rightLabel, valueChangedDelegate, getValueDelegate, () => s_provider.GetControlPrefab("Toggle"));
        }

        public static IControlTemplate CreateSliderTemplate(string title, Action<float> valueChangedDelegate, Func<float> getValueDelegate, (float minValue, float maxValue) range) {
            return new SliderControlTemplate(title, valueChangedDelegate, getValueDelegate, range, () => s_provider.GetControlPrefab("Slider"));
        }

        public static IControlTemplate CreateDropdownTemplate(string title, Action<int> valueChangedDelegate, Func<int> getValueDelegate, List<string> options) {
            return new DropdownControlTemplate(title, valueChangedDelegate, getValueDelegate, () => s_provider.GetControlPrefab("Dropdown"), options);
        }

        public static IControlTemplate CreateControl(string title, string controlID) {
            return new IndependentControlTemplate(title, () => s_provider.GetControlPrefab(controlID));
        }
    }
}