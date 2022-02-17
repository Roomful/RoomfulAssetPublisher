using System;

namespace net.roomful.api.settings
{
    public interface IRoomfulSettingsUIService
    {
        void Open(TabbedControlsHolder controlsHolder, Action openCallback, Action closeCallback);
        void Close();
        void SetDeleteButton(Action onClick);
    }
}
