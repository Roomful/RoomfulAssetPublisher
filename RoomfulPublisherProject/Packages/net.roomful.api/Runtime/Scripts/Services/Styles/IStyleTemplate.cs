using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IStyleTemplate : IRoomContentTemplate
    {
        IStyleAssetTemplate Asset { get; }
        List<IPanelTemplate> Panels { get; }
        string ParentStyleId { get; }
    }
}
