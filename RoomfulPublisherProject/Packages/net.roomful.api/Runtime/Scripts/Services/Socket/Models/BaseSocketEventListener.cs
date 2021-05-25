namespace net.roomful.api.socket
{
    public abstract class BaseSocketEventListener : ISocketEventListener
    {
        public abstract string Id { get; }
        public JSONData JSON { get; private set; }

        public void HandleData(JSONData data) {
            JSON = data;
        }
    }
}
