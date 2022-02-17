using net.roomful.api.ui;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Service gives accesses to the main app menu.
    /// </summary>
    public interface IAppMenuService
    {
        /// <summary>
        /// Room UI accesses point.
        /// </summary>
        IRoomUIView RoomUI { get; }

        /// <summary>
        /// Room Toolbar UI accesses point.
        /// </summary>
        ICustomizableView ToolbarUI { get; }

        /// <summary>
        /// App menu accesses point.
        /// </summary>
        ICustomizableView AppMenuUI { get; }

        /// <summary>
        /// Gives an ability to create new side menu instance.
        /// </summary>
        /// <param name="name">Name of the new size menu.</param>
        /// <returns></returns>
        ISidePanelController InstantiateSideMenu(string name);

        /// <summary>
        /// Gives an ability to add bottom button.
        /// </summary>
        /// <param name="buttonData">Data of the button data</param>
        /// <returns></returns>
        IButtonView AddBottomButton(ButtonData buttonData);
    }
}
