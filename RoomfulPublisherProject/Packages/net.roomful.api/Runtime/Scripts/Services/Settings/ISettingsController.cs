using System;

namespace net.roomful.api.settings
{
    public interface ISettingsController
    {
        void AddControl(string tabName, IControlTemplate template);
        void AddTab(string tabName, string label, int priority);
        event Action OnCloseRequest;
        void Close();
        void Open(TabbedControlsHolder controlsHolder, Action onClose);
        void SetDeleteButton(Action onClick);
    }
}