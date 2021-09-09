using System;
using System.Collections.Generic;
using net.roomful.api.props;
using net.roomful.api.sa;
using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api.native
{
    public interface INativePlatform
    {
        //--------------------------------------
        // Initialisation
        //--------------------------------------

        void InitNativeAPI(string accessToken, string serverUrl);
        SA_Event OnKeyboardWillAppear { get; }
        void SetCurrentUserInfo(IUserTemplate user);

        INativePluginEvents Events { get; }

        //--------------------------------------
        //  WEB  Platforms API
        //--------------------------------------

        void SignOut();
        void LobbyReady();
        void LandingLoaded();
        void SendToken(string token);
        void RoomLoadFailed();
        void SetDeviceId(string id);
        void ShowZoomView(ZoomViewModel zoomViewModel);
        void GenerateHistoryItem(string route, long timestamp);
        WebglPlatform GetWebglPlatform();

        //--------------------------------------
        //  Picker
        //--------------------------------------

        Action OnNativeContentPickerClosed { get; set; }

        void ShowSortingTablePicker(ContentPickerParams pickerParams);
        void ShowPicker(ContentPickerParams pickerParams);

        // New methods for picker old should be deleted --- --- ---
        void ShowSortingTablePicker(ContentPickerParams pickerParams, Action<string> callback);
        void ShowPicker(ContentPickerParams pickerParams, Action<string> callback);
        //  --- --- --- --- --- --- --- --- --- --- --- --- --- ---

        void ShowVideoCapture(Action<List<IResource>> callback, string title = "", string additionalParam = "");
        void ShowAudioCapture(Action<List<IResource>> callback, string title = "", string additionalParam = "");
        void GetLocalThumbnail(IResource res, ThumbnailSize size, Action<byte[]> callback);
        void ConvertAudioToWav(string resourceId, AudioClip clip, Action<byte[]> callback);

        //--------------------------------------
        //  Zoom View
        //--------------------------------------

        void PreviewResource(IResource resource, Action callback = null);
        void ShowWebView(UrlLink urlLink);
        void ShowYoutubeWebView(UrlLink link, Action callback = null);
        void HideModalWindow();
        //--------------------------------------
        //  Upload
        //--------------------------------------

        void RequestUpload(IResource[] res);

        //--------------------------------------
        //  Chat
        //--------------------------------------

        void StartVideoChatWithToken(IVideoChatToken videoChatToken);
        void OnVideoChatUpdated(IVideoChatStatus status);
        void OnVideoChatParticipantConnected(IChatParticipantModel participant);
        void OnVideoChatParticipantDisconnected(IChatParticipantModel participant);
        void OnVideoChatParticipantUpdated(IChatParticipantModel participant);
        void OnVideoChatPromotionDecline(IChatParticipantModel participant);
        void OnVideoChatPromotionRequest(IChatParticipantModel participant);
        void OnVideoChatTurnOffUserOptionUpdated(string videoChatId, int option);

        void VideoChatRemoveStream(IVideoChatConnection chatConnection);
        void OnVideoChatShareScreenStarted(IVideoChatConnection chatConnection);
        void OnVideoChatShareScreenStopped(IVideoChatConnection chatConnection);

        void VideoChatMuteAll(string videoChatId);
        void VideoChatUnMuteAll(string videoChatId);
        void VideoChatParticipantIdentityUpdated(string videochatId, string identity, int uid, int handStatus, int microphoneStatus);
        void StopVideoChat(string videochatId);
        void MinimizeChatControls();
        void RestoreChatControls();
        bool GetVideoChatControlsVisibility();

        //--------------------------------------
        //  Resources
        //--------------------------------------
        void VisitorJoinedRoom(string roomId, IUserTemplate user);

        //--------------------------------------
        //  Other
        //--------------------------------------

        bool IsFacebook { get; set; }
        bool ZoomViewDebugMode { get; set; }
        string DeviceId { get; }
        void CopyToClipboard(string text);
        void ShowCopyPasteMenu(string data, Action<CopyPasteMenuResult> callback);
        void GetFromClipboard(Action<string> callback);
        float GetKeyboardHeight();
        float GetScreenDPI();
        void OpenPdfRequest(string title, string roomId, string propId, string resourceId);

        //--------------------------------------
        //  Share
        //--------------------------------------

        void ShareRoom(string url, Texture2D text);
        void NotifyRoomEnter(IRoomTemplate template);
        Uri LaunchUrl { get; set; }
        SA_Event<string> OnPaymentClosed { get; }
        void SetBrowserCursor(CursorType cursorType);
        void OpenFullVersion();
        void ShowPaymentBox(string token);
        void ShowManageContentUI(ZoomViewModel zoomViewModel);
        void UpdateRoomResource(string roomId, string propId, IResource resource);
        void OpenWebGLFullScreen(bool value);


        //--------------------------------------
        //  Props
        //--------------------------------------

        void UpdatePropContent(IPropTemplate template);
        void UpdateProp(IPropTemplate template);
        event Action<ResourcePositionUpdate> OnPropResourcePositionUpdated;
    }
}
