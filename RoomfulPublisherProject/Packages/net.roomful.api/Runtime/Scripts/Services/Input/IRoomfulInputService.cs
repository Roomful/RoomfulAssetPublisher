namespace net.roomful.api
{
    public interface IRoomfulInputService
    {
        void SetKeyBoardInputLock(bool locked);
        void SimulateOnDragEvent(DragEventData eventData);

        IRoomfulInputDispatcher RoomInput { get; }
        IRoomfulInputDispatcher UInput { get; }
        float HorizontalSensitivity { get; }
        float VerticalSensitivity { get; }

        void EnableRoomInput();
        void DisabledRoomInput();

        bool IsRoomInputEnabled { get; }
    }
}
