using net.roomful.api.room;
using UnityEngine;

namespace net.roomful.api.appMenu
{
    public static class ICustomizableViewExtensions
    {
        public static float GetNormalizedAndScaledIndent(this ICustomizableView customizableView, int screenWidth) {
            switch (Roomful.RoomService.RoomMode) {
                case RoomUIMode.SimpleView:
                case RoomUIMode.EmptyUIMode:
                case RoomUIMode.EmptyUIModeWithSidePanelEnabled:
                    var indent = customizableView.GetIndent();
                    return (indent.indent-1) * indent.canvasScale.x / screenWidth;

                default:
                    return 0f;
            }
        }

        public static int GetScaledIndent(this ICustomizableView customizableView) {
            switch (Roomful.RoomService.RoomMode) {
                case RoomUIMode.SimpleView:
                case RoomUIMode.EmptyUIMode:
                case RoomUIMode.EmptyUIModeWithSidePanelEnabled:
                    var indent = customizableView.GetIndent();
                    return (int) ((indent.indent-1) * indent.canvasScale.x);

                default:
                    return 0;
            }
        }
    }
}