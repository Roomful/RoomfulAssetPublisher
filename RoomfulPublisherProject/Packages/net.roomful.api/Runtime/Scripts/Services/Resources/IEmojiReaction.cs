namespace net.roomful.api
{
    public interface IEmojiReaction
    {
        string UserId { get; }
        Emoji Emoji { get; }
    }
}
