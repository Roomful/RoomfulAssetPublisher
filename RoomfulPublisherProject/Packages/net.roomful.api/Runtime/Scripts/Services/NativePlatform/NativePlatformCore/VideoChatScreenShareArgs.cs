namespace net.roomful.api.native
{
    public struct SharedScreenInfo
    {
        public bool SystemAudioShared;

        public static SharedScreenInfo Empty => new SharedScreenInfo {
            SystemAudioShared = false
        };
    }

    public abstract class VideoChatScreenShareArgs
    {
        public string Data { get; }
        public string VideochatId { get; }

        protected readonly JSONData m_jsonData;

        protected VideoChatScreenShareArgs(string data) {
            Data = data;

            m_jsonData = new JSONData(Data);
            VideochatId = m_jsonData.GetValue<string>("videochatId");
        }
    }

    public sealed class VideoChatScreenShareRequestedArgs : VideoChatScreenShareArgs
    {
        public VideoChatScreenShareRequestedArgs(string data) : base(data) { }
    }

    public sealed class VideoChatScreenShareRequestDeclinedArgs : VideoChatScreenShareArgs
    {
        public VideoChatScreenShareRequestDeclinedArgs(string data) : base(data) { }
    }

    public sealed class VideoChatScreenShareStartedArgs : VideoChatScreenShareArgs
    {
        public SharedScreenInfo SharedScreenInfo { get; }

        public VideoChatScreenShareStartedArgs(string data) : base(data) {
            SharedScreenInfo = new SharedScreenInfo {
                SystemAudioShared = m_jsonData.GetValue<bool>("isAudioShared")
            };
        }
    }

    public sealed class VideoChatScreenShareEndedArgs : VideoChatScreenShareArgs
    {
        public VideoChatScreenShareEndedArgs(string data) : base(data) { }
    }
}