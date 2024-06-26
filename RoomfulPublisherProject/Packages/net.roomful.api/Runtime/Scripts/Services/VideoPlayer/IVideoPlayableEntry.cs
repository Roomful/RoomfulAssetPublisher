using System.Collections.Generic;

namespace net.roomful.api.player.video
{
    public enum VideoStreamType
    {
        Media,
        Hls
    }
    
    public interface IVideoPlayableEntry : IPlayableEntry
    {
        VideoStreamType StreamType { get; }

        IEnumerable<string> ISRCs { get; }

        IEnumerable<string> HFASongCodes { get; }
    }
}