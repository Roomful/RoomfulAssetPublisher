using net.roomful.api.props;

namespace net.roomful.api.zoom
{
    /// <summary>
    /// Used to override how props are linked during the zoom view
    /// prev / next walk
    /// </summary>
    public interface IZoomViewPropLinksProvider
    {
        string Name { get; }
        IProp GetNextProp(IProp current);
        IProp GetPrevProp(IProp current);
    }
}
