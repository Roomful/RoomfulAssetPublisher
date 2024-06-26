using System.Collections.Generic;
using net.roomful.api;
using StansAssets.Foundation;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public class EmojiReaction : IEmojiReaction
    {
        public string UserId { get; }
        public Emoji Emoji { get; set; }

        public EmojiReaction(JSONData emojiReactionInfo) {
            var emojiString = emojiReactionInfo.GetValue<string>("reaction");
            Emoji = EnumUtility.ParseEnum<Emoji>(emojiString);
            var authorInfo = new JSONData(emojiReactionInfo.GetValue<Dictionary<string, object>>("fromUser"));
            UserId = new UserDataModelSimple(authorInfo).Id;
        }

        public EmojiReaction(string userId, Emoji emoji) {
            Emoji = emoji;
            UserId = userId;
        }

        public void SetReaction(Emoji newReaction) {
            Emoji = newReaction;
        }

        public Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object> {
                { "reaction", Emoji.ToString() },
                { "userId", UserId }
            };
            return data;
        }
    }
}
