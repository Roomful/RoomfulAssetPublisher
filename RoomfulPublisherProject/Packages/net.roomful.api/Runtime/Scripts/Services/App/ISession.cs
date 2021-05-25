namespace net.roomful.api.app
{
    /// <summary>
    /// Roomful active session info
    /// </summary>
    public interface ISession
    {
        bool IsAnonymous { get; }
        bool IsNewUser { get; }
        string Id { get; }

        bool HasId();
    }
}
