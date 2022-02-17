using net.roomful.api.activity;
using net.roomful.api.app;
using net.roomful.api.appMenu;
using net.roomful.api.assets;
using net.roomful.api.authentication;
using net.roomful.api.avatars;
using net.roomful.api.avatars.emotions;
using net.roomful.api.cameras;
using net.roomful.api.colorization;
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
using net.roomful.api.scenes;
using net.roomful.api.settings;
using net.roomful.api.socket;
using net.roomful.api.styles;
using net.roomful.api.textChat;
using net.roomful.api.ui;
using net.roomful.api.videoPlayer;
using net.roomful.api.zoom;
using RF.Room.Teleportation;

namespace net.roomful.api
{
    // TODO all functions here has to be internal.
    public static partial class Roomful
    {
        public static void SetPropsService(IPropsService propsService) {
            PropsService = propsService;
        }

        public static void SetInputService(IRoomfulInputService inputService) {
            Input = inputService;
        }

        public static void SetNativePlatformService(INativePlatform nativePlatform) {
            Native = nativePlatform;
        }

        public static void SetAssetsService(IAssetsService assetsService) {
            Assets = assetsService;
        }

        public static void SetCameraService(IRoomCameraService roomCamera) {
            CameraService = roomCamera;
        }

        public static void SetTeleportationService(ITeleportationService teleportationService) {
            TeleportationService = teleportationService;
        }

        public static void SetStyleService(IStyleService styleService) {
            StyleService = styleService;
        }

        public static void SetLocalization(ILocalizationService localization) {
            Localization = localization;
        }

        public static void SetRoomService(IRoomService roomService) {
            RoomService = roomService;
        }

        public static void SetAuthentication(IAuthenticationService authenticationService) {
            Authentication = authenticationService;
        }

        public static void SetSocketService(ISocketService socketService) {
            SocketService = socketService;
        }

        public static void SetInRoomAvatarService(IInRoomAvatarService avatarService) {
            InRoomAvatarService = avatarService;
        }

        public static void SetInRoomAvatarEmotionsService(IEmotionsService avatarEmotionsService) {
            InRoomAvatarEmotionsService = avatarEmotionsService;
        }

        public static void SetPresenterBoardService(IPresenterBoardService presenterBoardService) {
            PresenterBoardService = presenterBoardService;
        }

        public static void SetLobbyService(ILobbyService lobbyService) {
            LobbyService = lobbyService;
        }

        public static void SetRoomSettingsUIService(IRoomSettingsUIService roomSettingsUIService) {
            RoomSettingsUIService = roomSettingsUIService;
        }

        public static void SetResourceSettingsUIService(IResourcesSettingsUIService roomSettingsUIService) {
            ResourceSettingsUIService = roomSettingsUIService;
        }

        public static void SetRoomInfoUIService(IRoomInfoUIService roomInfoUIService) {
            RoomInfoUIService = roomInfoUIService;
        }

        public static void SetUsersService(IUsersService usersService) {
            UsersService = usersService;
        }

        public static void SetZoomViewService(IZoomViewService zoomViewService) {
            ZoomView = zoomViewService;
        }

        public static void SetSession(ISession session) {
            Session = session;
        }

        public static void SetVideoChatService(IVideoChatService videoChatService) {
            VideoChatService = videoChatService;
        }

        public static void SetResourcesService(IResourcesService resourcesService) {
            ResourcesService = resourcesService;
        }

        public static void SetAppMenuService(IAppMenuService appMenuService) {
            AppMenuService = appMenuService;
        }

        public static void SetTextChat(IPublicTextChatService textChat) {
            TextChat = textChat;
        }

        public static void SetProfile(IProfileService profileService) {
            ProfileService = profileService;
        }

        public static void SetNetworksService(INetworksService networksService) {
            NetworksService = networksService;
        }

        public static void SetPaymentService(IPaymentService paymentService) {
            Payment = paymentService;
        }

        public static void SetVideoPlayerService(IVideoPlayerService videoPlayerService) {
            VideoPlayerService = videoPlayerService;
        }

        public static void SetUsersTrackingService(IUsersTrackingService usersTrackingService) {
            UsersTracking = usersTrackingService;
        }

        public static void SetRoutesService(IRoutesService routesService) {
            RoutesService = routesService;
        }

        public static void SetActivityService(IActivityService activity) {
            Activity = activity;
        }

        public static void SetColorizationService(IColorizationService colorizationService) {
            ColorizationService = colorizationService;
        }

        public static void SetSceneService(ISceneService sceneService) {
            Scenes = sceneService;
        }

        public static void SetUIService(IUIService uiService) {
            UI = uiService;
        }
    }
}
