namespace net.roomful.api
{
    public interface IRoomfulInputService
    {
        void SetKeyBoardInputLock(bool locked);
        void EnableRoomInput();
        void EnableARInput();

        void SimulateOnDragEvent(DragEventData eventData);

        IRoomfulInputDispatcher RoomInput { get; }
        IRoomfulInputDispatcher ArInput { get; }

        float HorizontalSensitivity { get; }
        float VerticalSensitivity { get; }
    }
}
