using System;
using UnityEngine;

namespace net.roomful.api.room
{
    /// <summary>
    /// Service allows interaction with currently loaded room,
    /// provides various room actions and info, and allows to subscribe to the room lifecycle.
    /// As well as ability to request new room load.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Event is triggered when room is loaded.
        /// </summary>
        event Action<IRoomTemplate> OnRoomLoaded;
        
        /// <summary>
        /// Event is triggered when room load is started.
        /// </summary>
        event Action<IRoomTemplate> OnRoomStartedLoading;

        /// <summary>
        /// Event is triggered when room is unloaded.
        /// </summary>
        event Action OnRoomUnloaded;

        /// <summary>
        /// Event is triggered when room is about to unload.
        /// </summary>
        event Action OnRoomAboutToUnload;

        /// <summary>
        /// Event is triggered when current room is updated.
        /// </summary>
        event Action<IRoomTemplate> OnCurrentRoomTemplateUpdated;

        /// <summary>
        /// Event is triggered when room mode is changed.
        /// </summary>
        event Action<RoomUIMode> OnRoomModeChanged;

        /// <summary>
        /// Currently loaded room model.
        /// The value is `null` if room hasn't been loaded yet.
        /// </summary>
        IRoom Room { get; }

        /// <summary>
        /// True if Room is open and <see cref="Room"/> property is not `null`.
        /// </summary>
        bool IsRoomOpened { get; }

        /// <summary>
        /// Room root transform.
        /// </summary>
        Transform RoomRoot { get; }

        /// <summary>
        /// Room metadata objects root transform.
        /// </summary>
        Transform RoomMeta { get; }

        /// <summary>
        /// Set room UI mode.
        /// </summary>
        /// <param name="mode">Room UI mode</param>
        void SetRoomMode(RoomUIMode mode);
        
        /// <summary>
        /// Open room by id, returns loaded room in callback
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="callback"></param>
        void OpenRoomById(string roomId, Action<IRoomTemplate> callback);
    }
}
