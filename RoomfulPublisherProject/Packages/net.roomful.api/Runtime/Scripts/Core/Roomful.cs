// Copyright Roomful 2013-2020. All rights reserved.

using net.roomful.api;
using net.roomful.api.app;
using net.roomful.api.appMenu;
using net.roomful.api.assets;
using net.roomful.api.authentication;
using net.roomful.api.avatars;
using net.roomful.api.avatars.emotions;
using net.roomful.api.cameras;
using net.roomful.api.lobby;
using net.roomful.api.localization;
using net.roomful.api.native;
using net.roomful.api.networks;
using net.roomful.api.payment;
using net.roomful.api.presentation.board;
using net.roomful.api.profile;
using net.roomful.api.props;
using net.roomful.api.resources;
using net.roomful.api.room;
using net.roomful.api.socket;
using net.roomful.api.styles;
using net.roomful.api.videoPlayer;
using net.roomful.api.zoom;
using RF.Room.Teleportation;
using UnityEngine;

namespace net.roomful
{
    public static partial class Roomful
    {
        /// <summary>
        /// Services provides user related events and API to interact with users.
        /// </summary>
        public static IUsersService UsersService { get; private set; }

        /// <summary>
        /// Roomful Session info
        /// </summary>
        public static ISession Session { get; private set; }

        /// <summary>
        /// Services provides prop related events and API to interact with props.
        /// </summary>
        public static IPropsService PropsService { get; private set; }

        /// <summary>
        /// An Accesses point to Roomful input.
        /// </summary>
        public static IRoomfulInputService Input { get; private set; }

        /// <summary>
        /// An Accesses point to the Room camera.
        /// </summary>
        public static IRoomCameraService CameraService { get; private set; }

        /// <summary>
        /// Service for teleportation purposes
        /// </summary>
        public static ITeleportationService TeleportationService { get; private set; }

        /// <summary>
        /// Platform native functions accesses point.
        /// </summary>
        public static INativePlatform Native { get; private set; }

        /// <summary>
        /// Service contains asset related functions.
        /// </summary>
        public static IAssetsService Assets { get; private set; }

        /// <summary>
        /// An Accesses point to room styles.
        /// </summary>
        public static IStyleService StyleService { get; private set; }

        /// <summary>
        /// Use this service to support localized content.
        /// </summary>
        public static ILocalizationService Localization { get; private set; }

        /// <summary>
        /// Service allows interaction with currently loaded room,
        /// provides various room actions and info, and allows to subscribe to the room lifecycle.
        /// As well as ability to request new room load.
        /// </summary>
        public static IRoomService RoomService { get; private set; }

        /// <summary>
        /// Allows to monitor application authentication related events,
        /// as well as control user authentication status.
        /// </summary>
        public static IAuthenticationService Authentication { get; private set; }

        /// <summary>
        /// Provides API to work with socket connection.
        /// </summary>
        public static ISocketService SocketService { get; private set; }

        /// <summary>
        /// An Accesses point to the in room 3D avatar's service.
        /// </summary>
        public static IInRoomAvatarService InRoomAvatarService { get; private set; }

        /// <summary>
        /// Service for playing emotions for 3D avatars.
        /// </summary>
        public static IEmotionsService InRoomAvatarEmotionsService { get; private set; }

        /// <summary>
        /// Service controls presentation boards.
        /// </summary>
        public static IPresenterBoardService PresenterBoardService { get; private set; }

        /// <summary>
        /// Service to inject custom UI into a Lobby
        /// </summary>
        public static ILobbyService LobbyService { get; private set; }

        /// <summary>
        /// Service to inject custom Room Settings
        /// </summary>
        public static IRoomSettingsUIService RoomSettingsUIService { get; private set; }
        
        /// <summary>
        /// Service to inject custom Room Info
        /// </summary>
        public static IRoomInfoUIService RoomInfoUIService { get; private set; }

        /// <summary>
        /// Service allows subscription to the zoom view events and adding custom behaviour.
        /// </summary>
        public static IZoomViewService ZoomView { get; private set; }

        /// <summary>
        /// Provides API to work with VideoChatService
        /// </summary>
        public static IVideoChatService VideoChatService { get; private set; }

        /// <summary>
        /// Services provides resources related events and API to interact with resources.
        /// </summary>
        public static IResourcesService ResourcesService { get; private set; }

        /// <summary>
        /// Gives access to the app Menu
        /// </summary>
        public static IAppMenuService AppMenuService { get; private set; }

        /// <summary>
        /// Gives access to the text chat api
        /// </summary>
        public static IPublicTextChatService TextChat { get; private set; }

        /// <summary>
        /// Used to display users profile
        /// </summary>
        public static IProfileService ProfileService { get; private set; }

        /// <summary>
        /// Use to monitor networks events.
        /// </summary>
        public static INetworksService NetworksService { get; private set; }

        /// <summary>
        /// Roomful payment service.
        /// </summary>
        public static  IPaymentService Payment  { get; private set; }

        /// <summary>
        /// Used for track video players state.
        /// </summary>
        public static  IVideoPlayerService VideoPlayerService  { get; private set; }

        /// <summary>
        /// Provides info about users current location in Roomful.
        /// </summary>
        public static  IUsersTrackingService UsersTracking  { get; private set; }

        public static RoomfulPlatform Platform {
            get {
                switch (Application.platform) {
                    case RuntimePlatform.WebGLPlayer:
                        return RoomfulPlatform.Web;
                    case RuntimePlatform.Android:
                    case RuntimePlatform.IPhonePlayer:
                        return RoomfulPlatform.Mobile;
                    default:
                        return RoomfulPlatform.Editor;
                }
            }
        }
    }
}
