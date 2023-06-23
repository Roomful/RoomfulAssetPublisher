using System.Collections.Generic;

namespace net.roomful.api.socket
{
    public interface ISocketPackage
    {
        string Id { get; }
        Dictionary<string, object> GenerateData();
    }
}
