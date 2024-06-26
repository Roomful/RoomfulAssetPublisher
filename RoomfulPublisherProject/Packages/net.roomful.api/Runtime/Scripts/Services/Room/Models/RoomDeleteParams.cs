namespace net.roomful.api.room
{
    public struct RoomDeleteParams
    {
        public string RoomId { get; }

        public RoomDeleteParams(string roomId) {
            RoomId = roomId;
        }
    }
}
