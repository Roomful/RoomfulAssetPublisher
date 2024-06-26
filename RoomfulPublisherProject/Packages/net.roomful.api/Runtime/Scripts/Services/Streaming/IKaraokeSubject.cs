using net.roomful.api.props;

namespace net.roomful.api.karaoke
{
    public enum State
    {
        Offline,
        Viewer,
        Singer
    }

    public static class VideoChatProperties
    {
        public static readonly string PlaybackState = "net.roomful.api.karaoke.PlaybackState";
    }

    public static class PlaybackActions
    {
        public static readonly string Unknown = "Unknown";
        public static readonly string Playing = "Playing";
        public static readonly string Paused = "Paused";
        public static readonly string Stopped = "Stopped";
        public static readonly string PlaybackEntryChanged = "PlaybackEntryChanged";
    }

    public readonly struct PlaybackStateChangedArgs
    {
        public readonly string Action;
        public readonly string PlaybackEntryId;
        public readonly string PlaybackEntryTitle;
        public readonly string PlaybackEntryArtist;
        
        public PlaybackStateChangedArgs(string action, string playbackEntryId, string playbackEntryTitle, string playbackEntryArtist) {
            Action = action;
            PlaybackEntryId = playbackEntryId;
            PlaybackEntryTitle = playbackEntryTitle;
            PlaybackEntryArtist = playbackEntryArtist;
        }
    }
    
    public delegate void PlaybackStateChanged(PlaybackStateChangedArgs args);

    public interface IKaraokeContext
    {
        event PlaybackStateChanged OnPlaybackStateChanged;
    }
    
    public interface IKaraokeSubject
    {
        IProp Owner { get; }
        IKaraokeContext Context { get; }
        void CompleteBootstrap();
        void SetState(State state);
        void Dispose();
    }
}