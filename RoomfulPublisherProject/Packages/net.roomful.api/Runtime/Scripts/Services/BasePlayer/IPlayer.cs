using System;
using System.Collections.Generic;

namespace net.roomful.api.player
{
    public interface IPlayableEntry
    {
        /// <summary>
        /// Song id in catalog
        /// </summary>
        string Id { get; }
        string Title { get; }
        string Artist { get; }
        IEnumerable<string> Writers { get; }
        IEnumerable<string> Genres { get; }
        string Decade { get; }
        /// <summary>
        /// Tonic note (chord)
        /// </summary>
        string Key { get; }
        /// <summary>
        /// Length in seconds
        /// </summary>
        double Length { get; }
        string BarIntro { get; }

        void GetUrlAsync(Action<string> callback);
    }

    public interface IPlayer
    {
        event Action OnMuteStateChanged;
        event Action OnVolumeUpdated;
        event Action OnContextUpdated;
        event Action<PlayerState> OnStateChanged;

        string Id { get; }
        PlayerState State { get; }

        /// <summary>
        /// Current video time in milliseconds.
        /// </summary>
        float CurrentTime { get; }
        
        /// <summary>
        /// Total Video duration time in milliseconds.
        /// </summary>
        float Duration { get; }
        
        bool Muted { get; set; }
        float Volume { get; set; }
        
        void PlayNext();
        void PlayPrevious();
        void Pause();
        void Resume();
        void Stop();
        void Seek(float positionMs);
        void Release();
    }
}
