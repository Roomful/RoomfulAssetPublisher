
namespace net.roomful.api.socket
{
    public interface ISocketRequestCallback
    {
        void HandleData(JSONData data);
        void HandleError(SocketError error);
    }
}
