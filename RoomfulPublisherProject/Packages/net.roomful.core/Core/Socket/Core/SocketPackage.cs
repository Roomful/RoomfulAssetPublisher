using System.Collections.Generic;

namespace net.roomful.api.socket
{
    public abstract class SocketPackage : ISocketPackage
    {
        public abstract string Id { get; }

        protected Dictionary<string, object> m_data;
        public Dictionary<string, object> GenerateData() => m_data;
    }
}