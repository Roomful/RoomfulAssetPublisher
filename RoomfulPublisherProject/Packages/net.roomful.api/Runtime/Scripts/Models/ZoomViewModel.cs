using System;
using net.roomful.api.props;

namespace net.roomful.api
{
    public class ZoomViewModel
    {
        public IPropTemplate PropTemplate { get; }
        public ZoomViewModeType ZoomViewMode { get; }
        public UrlLink UrlLink { get; }
        public string ResId { get; }
        public Action Callback { get; }

        public ZoomViewModel(
            IPropTemplate propTemplate,
            ZoomViewModeType zoomViewMode,
            UrlLink urlLink,
            string resId = "",
            Action callback = null) {
            PropTemplate = propTemplate;
            ZoomViewMode = zoomViewMode;
            UrlLink = urlLink;
            ResId = resId;
            Callback = callback;
        }
    }
}
