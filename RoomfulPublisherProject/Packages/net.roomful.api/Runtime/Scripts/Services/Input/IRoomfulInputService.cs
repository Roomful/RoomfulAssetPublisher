using System;

namespace net.roomful.api
{
    public interface IRoomfulInputService
    {
        event Action OnRoomInputDisabled;
        event Action OnRoomInputEnabled;
        void SetKeyBoardInputLock(bool locked);
        void SimulateOnDragEvent(DragEventData eventData);

        IRoomfulInputDispatcher RoomInput { get; }
        IRoomfulInputDispatcher UInput { get; }
        float HorizontalSensitivity { get; }
        float VerticalSensitivity { get; }

        void EnableRoomInput(InputFlags flags = InputFlags.All);
        void DisabledRoomInput(InputFlags flags = InputFlags.All);

        InputFlags CurrentDisabledFlags { get; }
        bool IsRoomInputEnabled { get; }
    }
}
