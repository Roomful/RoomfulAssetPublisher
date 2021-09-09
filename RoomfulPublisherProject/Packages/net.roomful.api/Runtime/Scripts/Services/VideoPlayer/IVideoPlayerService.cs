using System;

namespace net.roomful.api.videoPlayer {
    /// <summary>
    /// Used to track the states of the video player
    /// </summary>
    public interface IVideoPlayerService {
        void RegisterVideoPlayer(IVideoPlayer player);
        
        void DeregisterVideoPlayer(IVideoPlayer player);

        event Action<VideoPlayerState, IVideoPlayer> OnVideoPlayerStateChanged;
    }
}