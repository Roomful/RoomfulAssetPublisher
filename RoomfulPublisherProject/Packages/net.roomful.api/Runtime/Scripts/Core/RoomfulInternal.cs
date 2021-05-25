using net.roomful.api;
using net.roomful.api.app;
using net.roomful.api.assets;
using net.roomful.api.authentication;
using net.roomful.api.avatars;
using net.roomful.api.avatars.emotions;
using net.roomful.api.cameras;
using net.roomful.api.lobby;
using net.roomful.api.localization;
using net.roomful.api.native;
using net.roomful.api.presentation.board;
using net.roomful.api.props;
using net.roomful.api.room;
using net.roomful.api.socket;
using net.roomful.api.styles;
using net.roomful.api.zoom;

namespace net.roomful
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

        public static void SetInRoomAvatarEmotionsService(IInRoomAvatarsEmotionService avatarEmotionsService) {
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

        public static void SetUsersService(IUsersService usersService) {
            UsersService = usersService;
        }

        public static void SetZoomViewService(IZoomViewService zoomViewService) {
            ZoomViewService = zoomViewService;
        }

        public static void SetSession(ISession session) {
            Session = session;
        }
        
        public static void SetVideoChatService(IVideoChatService videoChatService) {
            VideoChatService = videoChatService;
        }
    }
}
