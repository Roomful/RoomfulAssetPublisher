namespace net.roomful.api.socket
{
    /// <summary>
    /// Socket event listener.
    /// </summary>
    public interface ISocketEventListener
    {
        /// <summary>
        /// Event id to subscribe to.
        /// </summary>
        string Id { get; }

        void HandleData(JSONData data);
    }
}
