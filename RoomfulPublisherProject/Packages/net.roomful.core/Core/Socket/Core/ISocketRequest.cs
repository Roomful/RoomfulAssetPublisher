namespace net.roomful.api.socket
{
    public interface ISocketRequest<T> : ISocketPackage where T : ISocketRequestCallback, new()
    { }
}
