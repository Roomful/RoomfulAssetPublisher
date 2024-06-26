using System;
using net.roomful.api.appMenu;
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
        /// Event is triggered when room is loaded.
        /// And also all assets like styles and props has also been loaded for that room.
        /// </summary>
        event Action<IRoomTemplate> OnRoomLoadedWithAssets;

        /// <summary>
        /// Event is triggered when room load is started.
        /// </summary>
        event Action<IBaseRoomTemplate> OnRoomStartedLoading;

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
        /// Current RoomUI instance. Use it to access UI related to room.
        /// </summary>
        IRoomUI RoomUI { get; }

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
        /// Current Room UI mode.
        /// If room ui is not yer ready, the <see cref="RoomUIMode.Undefined"/> is returned.
        /// </summary>
        RoomUIMode RoomMode { get; }

        /// <summary>
        /// Open room by id, returns loaded room in callback
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="callback"></param>
        void OpenRoomById(string roomId, Action<IRoomTemplate> callback = null);

        /// <summary>
        /// Add room options, that will appear in the room settings window.
        /// </summary>
        /// <param name="tabName">Room Settings tab name.</param>
        /// <param name="title">Options Title.</param>
        /// <param name="roomParamName">Room `bool` param name.</param>
        /// <param name="valueChangedDelegate">
        /// Value change delegate.
        /// It will be called when user changes the value in the settings window,
        /// or when this value was changed via the network.
        /// </param>
        void AddRoomFlag(string tabName, string title, string roomParamName, Action<bool> valueChangedDelegate);

        string GetUrlFroRoom(string roomId);

        void ReturnBackToOpenRoom();

        /// <summary>
        /// Delete current opened room.
        /// </summary>
        void DeleteRoom();

        /// <summary>
        /// Quit current open room and return to lobby.
        /// </summary>
        void ExitRoom();

        /// <summary>
        /// Get info about the room by it's id.
        /// </summary>
        /// <param name="roomId">An id of the requested room.</param>
        /// <param name="callback">Room info callback.</param>
        void GetRoomInfo(string roomId, Action<IBaseRoomTemplate> callback);

        /// <summary>
        /// Load Room Thumbnail image.
        /// </summary>
        void GetRoomThumbnail(string roomId, ThumbnailSize size, Action<Texture2D> callback);
    }
}
