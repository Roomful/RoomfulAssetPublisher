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
    }
}
