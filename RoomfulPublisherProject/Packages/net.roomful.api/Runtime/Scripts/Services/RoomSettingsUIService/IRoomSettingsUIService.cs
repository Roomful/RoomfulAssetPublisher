using System;
using System.Collections.Generic;

namespace net.roomful.api.room
{

    public interface IRoomSettingsUIService
    {
        event Action OnBecomeVisible;

        void AddToggle(string tabName, string title, Action<bool> valueChangedDelegate, Func<bool> getValueDelegate);

        void AddSlider(string tabName, string title, Action<float> valueChangedDelegate, Func<float> getValueDelegate,
            (float minValue, float maxValue) range);

        void AddDropdown(string tabName, string title, Action<int> valueChangedDelegate, Func<int> getValueDelegate, List<string> options);

        void RemoveControl(string tabName, string title);
    }
}
