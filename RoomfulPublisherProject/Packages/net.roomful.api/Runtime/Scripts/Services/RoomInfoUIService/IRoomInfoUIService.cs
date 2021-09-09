using System;

namespace net.roomful.api.room
{
    public interface IRoomInfoUIService
    {
        event Action OnBecomeVisible;

        void AddTab(string tabName, string label, int priority);

        void AddControl(string tabName, string title, string controlID);

        void RemoveControl(string tabName, string title);

        bool IsExistTab(string tabName);
    }
}
