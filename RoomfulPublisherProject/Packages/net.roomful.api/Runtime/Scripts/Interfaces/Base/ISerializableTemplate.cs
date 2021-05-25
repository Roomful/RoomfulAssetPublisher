using System.Collections.Generic;

namespace net.roomful.api
{
    public interface ISerializableTemplate : ITemplate
    {
        Dictionary<string, object> ToDictionary();
    }
}