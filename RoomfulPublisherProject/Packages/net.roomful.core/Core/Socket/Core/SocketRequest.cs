using System.Collections.Generic;

namespace net.roomful.api.socket
{
    public abstract class SocketRequest<T> : ISocketRequest<T> where T : ISocketRequestCallback, new()
    {
        public abstract string Id { get; }

        protected Dictionary<string, object> m_data;
        public Dictionary<string, object> GenerateData() => m_data;
    }
}