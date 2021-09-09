using System;
using UnityEngine;

namespace net.roomful.api.room
{
    public static class RoomServiceExtensions
    {
        public static bool IsInsideRoomWithId(this IRoomService roomService, string roomId) {
            try {
                return roomService.IsRoomOpened && roomService.Room.Template.Id.Equals(roomId);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                return false;
            }
        }
    }
}
