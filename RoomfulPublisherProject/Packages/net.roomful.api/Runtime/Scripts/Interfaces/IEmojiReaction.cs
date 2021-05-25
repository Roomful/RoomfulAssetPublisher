using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IEmojiReaction
    {
        IUserTemplate User { get; }
        Emoji Reaction { get; }
        Emoji Emoji{ get; }
        DateTime LastUpdate { get; }
        void SetReaction(Emoji newReaction);
        Dictionary<string, object> ToDictionary();
    }
}