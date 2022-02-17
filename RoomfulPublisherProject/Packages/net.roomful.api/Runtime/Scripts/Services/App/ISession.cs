namespace net.roomful.api.app
{
    /// <summary>
    /// Roomful active session info
    /// </summary>
    public interface ISession
    {
        /// <summary>
        /// Returns `true` if current user is Anonymous and `false otherwise.`
        /// </summary>
        bool IsAnonymous { get; }

        /// <summary>
        /// Returns `true` if this is first user session in roomful `false otherwise.`
        /// </summary>
        bool IsNewUser { get; }

        /// <summary>
        /// Session id.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///  Give ability to check if session has server assigned id.
        /// </summary>
        /// <returns></returns>
        bool HasId();
    }
}
