namespace net.roomful.api.native
{
    public abstract class VideoPlayerEventArgs
    {
        public string Data { get; }
        public string PlayerId { get; }
        
        protected readonly JSONData m_jsonData;

        protected VideoPlayerEventArgs(string data) {
            Data = data;

            m_jsonData = new JSONData(Data);
            PlayerId = m_jsonData.GetValue<string>("playerId");
        }
    }

    public sealed class VideoPlayerReadyArgs : VideoPlayerEventArgs
    {
        public bool IsLoaded { get; }
        public float DurationMs { get; }

        public VideoPlayerReadyArgs(string data) : base(data) {
            IsLoaded = m_jsonData.GetValue<bool>("loaded");
            DurationMs = m_jsonData.GetValue<float>("duration") * 1000;
        }
    }
    
    public sealed class VideoPlayerProgressArgs : VideoPlayerEventArgs
    {
        public float PositionMs { get; }
        
        public VideoPlayerProgressArgs(string data) : base(data) {
            PositionMs = m_jsonData.GetValue<float>("progress") * 1000;
        }
    }
    
    public sealed class VideoPlayerPlaybackFinishedArgs : VideoPlayerEventArgs
    {
        public VideoPlayerPlaybackFinishedArgs(string data) : base(data) { }
    }
}