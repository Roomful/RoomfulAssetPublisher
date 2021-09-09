using net.roomful.api.ui;

namespace net.roomful.api.appMenu
{
    public interface IAppMenuService
    {
        IRoomUIView RoomUI { get; }
        ICustomizableView ToolbarUI { get; }
        ICustomizableView AppMenuUI { get; }
        ISidePanelController InstantiateSideMenu(string name);
        IButtonView AddBottomButton(ButtonData buttonData);
    }
}
