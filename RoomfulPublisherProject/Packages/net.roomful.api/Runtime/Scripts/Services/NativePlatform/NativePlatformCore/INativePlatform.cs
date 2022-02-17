﻿using System;
using net.roomful.api.props;
using net.roomful.api.sa;
using net.roomful.api.story;
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
        void SetCurrentUserInfo(IUserTemplate user);

        INativePluginEvents Events { get; }
        SA_Event OnKeyboardWillAppear { get; }

        //--------------------------------------
        //  WEB  Platforms API
        //--------------------------------------

        void SignOut();
        void LobbyReady();
        void LandingLoaded();
        void SendToken(string token);
        void RoomLoadFailed();
        void SetDeviceId(string id);
        void GenerateHistoryItem(string route, long timestamp);
        WebglPlatform GetWebglPlatform();

        //--------------------------------------
        //  Picker
        //--------------------------------------

        Action OnNativeContentPickerClosed { get; set; }

        void ShowPicker(ContentPickerParams pickerParams);
        void ConnectWallet();

        void ShowAudioCapture(Action<AudioResource> callback);
        void ConvertAudioToWav(string resourceId, AudioClip clip, Action<byte[]> callback);

        //--------------------------------------
        //  Zoom View
        //--------------------------------------

        void PreviewResource(IResource resource, Action callback = null);
        void ShowWebView(UrlLink urlLink);
        void ShowYoutubeWebView(UrlLink link);
        void ShowZoomView(WebZoomViewOpenParams webZoomViewOpenParams);

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
        //  Other
        //--------------------------------------

        bool IsFacebook { get; set; }
        bool IsUserInputPerformed { get; set; }
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
        string LaunchUrl { get; set; }
        SA_Event<string> OnPaymentClosed { get; }
        void ShowPaymentBox(string token);

        /// <summary>
        /// Will close any active native modal window/view.
        /// If no window/view is on display -> nothing should happen.
        /// </summary>
        void CloseNativeView();
        void UpdateRoomResource(string roomId, string propId, IResource resource);

        //--------------------------------------
        //  Props
        //--------------------------------------

        void UpdatePropContent(IPropTemplate template);
        void UpdateProp(IPropTemplate template);
        event Action<ResourcePositionUpdate> OnPropResourcePositionUpdated;
    }
}
