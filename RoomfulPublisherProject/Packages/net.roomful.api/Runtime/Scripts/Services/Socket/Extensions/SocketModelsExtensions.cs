using System;

namespace net.roomful.api.socket
{
    public static class SocketModelsExtensions
    {
        public static void Send(this ISocketPackage socketPackage) {
            Roomful.SocketService.Send(socketPackage);
        }

        public static void Send<T>(this ISocketRequest<T> socketRequest, Action<T> callback) where T : ISocketRequestCallback, new() {
            Roomful.SocketService.Send(socketRequest, callback);
        }
    }
}
