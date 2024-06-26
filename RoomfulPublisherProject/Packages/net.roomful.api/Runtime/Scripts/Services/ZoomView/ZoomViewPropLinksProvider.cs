using System;
using net.roomful.api.props;

namespace net.roomful.api.zoom
{
    public class ZoomViewPropLinksProvider : IZoomViewPropLinksProvider
    {
        public string Name { get; }

        private readonly Func<IProp, IProp> m_getNextPropDelegate;
        private readonly Func<IProp, IProp> m_getPrevPropDelegate;

        public ZoomViewPropLinksProvider(string name, Func<IProp, IProp> getNextPropDelegate, Func<IProp, IProp> getPrevPropDelegate) {
            Name = name;
            m_getNextPropDelegate = getNextPropDelegate;
            m_getPrevPropDelegate = getPrevPropDelegate;
        }

        public IProp GetNextProp(IProp current) {
            return m_getNextPropDelegate.Invoke(current);
        }

        public IProp GetPrevProp(IProp current) {
            return m_getPrevPropDelegate.Invoke(current);
        }
    }
}
