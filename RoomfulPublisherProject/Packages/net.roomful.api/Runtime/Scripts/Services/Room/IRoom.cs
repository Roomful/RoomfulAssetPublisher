using UnityEngine;

namespace net.roomful.api.room
{
    /// <summary>
    /// Model allow to read an manipulate the room.
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// Room template.
        /// </summary>
        IRoomTemplate Template { get; }

        /// <summary>
        /// Returns `true` if current user can edit a room.
        /// </summary>
        bool UserHasEditPermissions { get; }


        /// <summary>
        /// User on behalf we will present the room.
        /// If presenter wasn't set this is always will be a first owner.
        /// </summary>
        string RoomPresenterId { get; }

        /// <summary>
        /// Default room position where camera will be placed,
        /// unless custom home point is defined.
        /// </summary>
        Vector3 RoomInitialPosition { get; }

        void SetParam(string paramKey, object paramValue);
        T GetParam<T>(string paramKey);
        
    }
}
