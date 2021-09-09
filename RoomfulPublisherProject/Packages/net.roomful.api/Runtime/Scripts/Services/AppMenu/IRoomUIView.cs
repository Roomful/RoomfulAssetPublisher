using UnityEngine.UI;

namespace net.roomful.api.appMenu
{
    public interface IRoomUIView : ICustomizableView
    {
        void AddBottomRightLayoutElement(LayoutElement layoutElement, ElementOrderPriority priority = ElementOrderPriority.End);
    }
}
