using System;

namespace net.roomful.api.native
{
    /// <summary>
    /// NativeEventsDispatcher is a default implementation of the INativePluginEvents, to be used across the native platforms
    /// for native events listening, unless any specific logic needs to be implemented on top of it.
    /// </summary>
    public class NativeEventsDispatcher : INativePluginEvents
    {
        public event Action<MicStatusModel> OnSetMicrophoneStatusRequest;
        public event Action<ShowPopupModel> OnShowPopup;
        public event Action<RequestPromotionModel> OnRequestPromotion;
        public event Action<ConnectedToVideoChatModel> OnConnectedToVideoChat;
        public event Action<NativeContentPickedModel> OnContentPicked;
        public event Action<DispatchedDeepLinkModel> OnHandleDeepLinkEvent;
        public event Action<string> OnVideochatVideoStreamRemoved;
        public event Action<MuteAllModel> OnMuteAllEvent;
        public event Action<VideoChatHandStatusModel> OnHandStatusChangedEvent;
        public event Action<ChangeUserPermissionsModel> OnUserPermissionChanged;  
        public event Action OnVideoChatHasBeenClosed;
        public event Action OnVideoChatControlsUpdated;
        public event Action<string> OnVideoChatClosedForAll;
        public event Action<VideoPlayerReadyArgs> OnVideoPlayerReady = delegate { };
        public event Action<VideoPlayerProgressArgs> OnVideoPlayerProgress = delegate { };
        public event Action<VideoPlayerPlaybackFinishedArgs> OnVideoPlayerPlaybackFinished = delegate { };
        public event Action<VideoChatScreenShareRequestedArgs> OnVideoChatScreenShareRequested = delegate { };
        public event Action<VideoChatScreenShareRequestDeclinedArgs> OnVideoChatScreenShareRequestDeclined = delegate { };
        public event Action<VideoChatScreenShareStartedArgs> OnVideoChatScreenShareStarted = delegate { };
        public event Action<VideoChatScreenShareEndedArgs> OnVideoChatScreenShareEnded = delegate { };
        public event Action<CameraCaptureRequestCompletedArgs> OnCaptureDeviceCompleted = delegate { };
        public event Action<CameraCaptureRequestFailedArgs> OnCaptureDeviceFailed = delegate { };
        
        public event Action OnUserInputPerfomed;
        
        public void DispatchCaptureDeviceCompleted(CameraCaptureRequestCompletedArgs args) {
            OnCaptureDeviceCompleted(args);
        }
        
        public void DispatchCaptureDeviceFailed(CameraCaptureRequestFailedArgs args) {
            OnCaptureDeviceFailed(args);
        }

        public void DispatchVideoPlayerReady(VideoPlayerReadyArgs args) {
            OnVideoPlayerReady(args);
        }
        
        public void DispatchVideoPlayerProgress(VideoPlayerProgressArgs args) {
            OnVideoPlayerProgress(args);
        }
        
        public void DispatchVideoPlayerStop(VideoPlayerPlaybackFinishedArgs args) {
            OnVideoPlayerPlaybackFinished(args);
        }

        public void DispatchVideoChatScreenShareRequested(VideoChatScreenShareRequestedArgs args) {
            OnVideoChatScreenShareRequested(args);
        }
        
        public void DispatchVideoChatScreenShareRequestDeclined(VideoChatScreenShareRequestDeclinedArgs args) {
            OnVideoChatScreenShareRequestDeclined(args);
        }
        
        public void DispatchVideoChatScreenShareStarted(VideoChatScreenShareStartedArgs args) {
            OnVideoChatScreenShareStarted(args);
        }
        
        public void DispatchVideoChatScreenShareEnded(VideoChatScreenShareEndedArgs args) {
            OnVideoChatScreenShareEnded(args);
        }

        public void RequestSetMicrophoneStatus(MicStatusModel inputData) {
            OnSetMicrophoneStatusRequest?.Invoke(inputData);
        }

        public void RequestShowPopup(ShowPopupModel model) {
            OnShowPopup?.Invoke(model);
        }

        public void RequestPromotion(RequestPromotionModel inputData) {
            OnRequestPromotion?.Invoke(inputData);
        }

        public void NotifyConnectedToVideoChat(ConnectedToVideoChatModel inputData) {
            OnConnectedToVideoChat?.Invoke(inputData);
        }

        public void NotifyVideoChatClosed() {
            OnVideoChatHasBeenClosed?.Invoke();
        }

        public void NotifyContentPicked(NativeContentPickedModel inputData) {
            OnContentPicked?.Invoke(inputData);
        }

        public void OnHandleDeepLinkPayload (DispatchedDeepLinkModel model) {
            OnHandleDeepLinkEvent?.Invoke(model);
        }

        public void OnMuteAll(MuteAllModel model) {
            OnMuteAllEvent?.Invoke(model);
        }

        public void OnHandStatusChanged(VideoChatHandStatusModel model) {
            OnHandStatusChangedEvent?.Invoke(model);
        }

        public void NotifyUserPermissionsChanged(ChangeUserPermissionsModel model) {
            OnUserPermissionChanged?.Invoke(model);
        }

        public void NotifyVideoStreamRemoved(string videoChatId) {
            OnVideochatVideoStreamRemoved?.Invoke(videoChatId);
        }

        public void NotifyVideoChatControlsUpdated() {
            OnVideoChatControlsUpdated?.Invoke();
        }

        public void NotifyVideoChatClosedForAll (string videochatId) {
            OnVideoChatClosedForAll?.Invoke(videochatId);
        }

        public void NotifyUserInputPerformed() {
            OnUserInputPerfomed?.Invoke();
        }
    }
}
