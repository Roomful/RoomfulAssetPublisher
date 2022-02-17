using UnityEngine;

namespace net.roomful.api
{
    public static class UserRoomPositionExtensions
    {
        public static bool IsInsideTheProp(this IUserRoomPosition roomPosition) {
            return !string.IsNullOrEmpty(roomPosition.PropId);
        }

        public static bool IsEqualsTo(this IUserRoomPosition position, IUserRoomPosition other) {
            return position.RoomId == other.RoomId
                   && (position.Position.Position - other.Position.Position).sqrMagnitude < Mathf.Epsilon;
        }
    }
}
