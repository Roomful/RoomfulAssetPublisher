// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api.props
{
    public interface IPropComponent
    {
        void Init(IProp prop, int componentIndex);
        void OnPropUpdated();
        void PropScaleChanged();
        
        /// <summary>
        /// Invoked by zoom view service when zoom view is open.
        /// </summary>
        void OnZoomViewOpen();
        
        /// <summary>
        /// Invoked by zoom view service when zoom view is closed.
        /// </summary>
        void OnZoomViewClosed();
    }
}
