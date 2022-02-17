using UnityEngine.UI;

namespace net.roomful.api.appMenu
{
    /// <summary>
    /// Provide API to interact with the Room UI View.
    /// </summary>
    public interface IRoomUIView : ICustomizableView
    {
        void AddBottomRightLayoutElement(LayoutElement layoutElement);
    }
}
