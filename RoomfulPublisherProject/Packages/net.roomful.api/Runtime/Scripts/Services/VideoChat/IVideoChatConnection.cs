namespace net.roomful.api
{
    /// <summary>
    /// Model represents single video chat connection.
    /// </summary>
    public interface IVideoChatConnection
    {
        string VideochatId { get; }
        string UserId { get; }
        string Identity { get; }
        int Uid { get; }
    }
}