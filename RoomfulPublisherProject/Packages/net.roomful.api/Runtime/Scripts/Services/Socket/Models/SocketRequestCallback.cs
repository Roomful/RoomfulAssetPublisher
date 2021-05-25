namespace net.roomful.api.socket
{
    /// <summary>
    /// Default Request callback model.
    /// </summary>
    public class SocketRequestCallback : ISocketRequestCallback
    {
        public JSONData JSON { get; private set; }

        public void HandleData(JSONData data) {
            JSON = data;
        }

        public void HandleError(SocketError error) {

        }
    }
}
