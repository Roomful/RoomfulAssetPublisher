using System;

namespace net.roomful.api.native
{
    /// <summary>
    /// INativePluginEvents declares API for communication between a native plugin and the main project
    /// </summary>
    public interface INativePluginEvents
    {
        /// <summary>
        /// Notifies about a microphone status changes.
        /// </summary>
        /// <returns>SendMicStatusModel - a data class, required for the mic status update</returns>
        event Action<MicStatusModel> OnSetMicrophoneStatusRequest;

        /// <summary>
        /// States that a popup have to be shown
        /// </summary>
        /// <returns>ShowPopupModel is a popup content holder</returns>
        event Action<ShowPopupModel> OnShowPopup;

        /// <summary>
        /// Notifies about a promotion request
        /// </summary>
        /// <returns>RequestPromotionModel - a data class, required for the promotion request</returns>
        event Action<RequestPromotionModel> OnRequestPromotion;

        /// <summary>
        /// An event that notifies about the successful establishing of a video chat connection
        /// </summary>
        /// <returns>ConnectedToVideoChatModel is a data class, that returns an id of the video chat,
        /// local user has been connected to</returns>
        event Action<ConnectedToVideoChatModel> OnConnectedToVideoChat;

        /// <summary>
        /// Notifies that the native content has been picked by local user
        /// </summary>
        /// <returns>NativeContentPickedModel holds picked content uris</returns>
        event Action<NativeContentPickedModel> OnContentPicked;

        /// <summary>
        /// Native callback is being sent upon a deep link interception by the native plugin
        /// </summary>
        /// <returns>DispatchedDeepLinkModel contains deep link payload</returns>
        event Action<DispatchedDeepLinkModel> OnHandleDeepLinkEvent;

        /// <summary>
        /// Notifies that the video chat stream was removed.
        /// Event contains string video chat id.
        /// </summary>
        event Action<string> OnVideochatVideoStreamRemoved;

        /// <summary>
        /// Native callback to mute all participants in conference.
        /// </summary>
        /// <returns><c>MuteAllModel</c> contains video chat id and also bool value for state.</returns>
        event Action<MuteAllModel> OnMuteAllEvent;

        /// <summary>
        /// Native callback that called when participant change his hand status.
        /// </summary>
        /// <returns><c>VideoChatHandStatusModel</c> contains video shat id and also int for hand status.</returns>
        event Action<VideoChatHandStatusModel> OnHandStatusChangedEvent; 
        
        /// <summary>
        /// Notfies that the video chat has been closed
        /// from native controlls.
        event Action OnVideoChatHasBeenClosed;
        
    }
}
