namespace net.roomful.api.room
{
    public static class RoomServiceExtensions
    {
        public static bool IsInsideRoomWithId(this IRoomService roomService, string roomId) {
            return roomService.IsRoomOpened && roomService.Room.Template.Id.Equals(roomId);
        }
    }
}
