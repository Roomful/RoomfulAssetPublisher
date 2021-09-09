using System;

namespace net.roomful.api.videoPlayer
{
    public interface IVideoPlayer
    {
        VideoPlayerState State { get; }
        event Action<VideoPlayerState, IVideoPlayer> OnVideoPlayerStateChanged;
        void Play(IResource resource);
        void Disable();
        void Pause();
        void Resume();

        /// <summary>
        /// Returns `true` when pause is requested.
        /// Please note that <see cref="State"/> can be for example <see cref="VideoPlayerState.Loading"/>
        /// but pause was requested.
        ///
        /// At the same time will always return `true`when <see cref="State"/>  is <see cref="VideoPlayerState.Paused"/>
        /// </summary>
        bool IsPauseRequested { get; }

        /// <summary>
        /// Set video position.
        /// Position should be from 0 to 1.
        /// </summary>
        void SetVideoPosition(float position);

        /// <summary>
        /// Current video time in milliseconds.
        /// </summary>
        float CurrentTimeMs { get; }

        /// <summary>
        /// Total Video duration time in milliseconds.
        /// </summary>
        float DurationTimeMs { get; }

        /// <summary>
        /// Set volume as float value from 0 to 1
        /// </summary>
        void SetVolume(float volume);
    }
}
