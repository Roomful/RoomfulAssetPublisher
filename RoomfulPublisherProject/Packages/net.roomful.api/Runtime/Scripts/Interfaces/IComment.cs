using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IComment : ITemplate
    {
        int TotalAvaliableReactions { get; set;  }
        List<IEmojiReaction> Reactions { get; }
        Emoji Reaction { get; set; }
        DateTime LastUpdate { get; }
        DateTime Creation { get; }
        string Message { get; }
        List<IResource> Attachments { get; }
        IUserTemplate Author { get; }
        CommentState State { get; }
        float Length { get; set; }

        Dictionary<string, object> ToDictionary();
        void UpdateMeta(IComment comment);
    }
}