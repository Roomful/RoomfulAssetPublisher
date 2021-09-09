using System;
using System.Collections.Generic;

namespace net.roomful.api.room
{

    public interface IRoomSettingsUIService
    {
        event Action OnBecomeVisible;

        void AddTab(string tabName, string label, int priority);
        
        void AddCheckbox(string tabName, string title, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate);
        
        void AddToggle(string tabName, string title, string leftLabel, string rightLabel, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate);

        void AddSlider(string tabName, string title, Action<float> valueChangedDelegate, Func<float> getValueDelegate, (float minValue, float maxValue) range);

        void AddDropdown(string tabName, string title, Action<int> valueChangedDelegate, Func<int> getValueDelegate, List<string> options);

        void AddControl(string tabName, string title, string controlID);

        void AddEditField(string tabName, string title, Action<string> valueChangedDelegate, Func<string> getValueDelegate, Func<string, string> validator);

        void RemoveControl(string tabName, string title);
        
        bool IsExistTab(string tabName);
    }
}
