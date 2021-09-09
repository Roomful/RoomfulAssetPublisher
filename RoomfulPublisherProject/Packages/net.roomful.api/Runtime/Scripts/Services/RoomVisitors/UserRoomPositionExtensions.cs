namespace net.roomful.api
{
    public static class UserRoomPositionExtensions
    {
        public static bool IsInsideTheProp(this IUserRoomPosition roomPosition) {
            return !string.IsNullOrEmpty(roomPosition.PropId);
        }
    }
}
