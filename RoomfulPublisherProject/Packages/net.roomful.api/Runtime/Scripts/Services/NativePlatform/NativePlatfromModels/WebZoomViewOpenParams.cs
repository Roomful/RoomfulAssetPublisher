using System.Text;
using net.roomful.api.props;

namespace net.roomful.api.native
{
    public class WebZoomViewOpenParams
    {
        public IPropTemplate PropTemplate { get; }
        public ZoomViewModeType ZoomViewMode { get; }
        public UrlLink UrlLink { get; }

        /// <summary>
        /// Route will be set when coming back from the popup.
        /// </summary>
        public string BackRoute { get; }
        public bool IsPromo { get; }


        public WebZoomViewOpenParams(IPropTemplate propTemplate,
            ZoomViewModeType zoomViewMode, string backRoute, bool isPromo = false) {

            PropTemplate = propTemplate;
            ZoomViewMode = zoomViewMode;
            BackRoute = backRoute;
            IsPromo = isPromo;
            UrlLink = GenerateWebUrl();
        }

        private UrlLink GenerateWebUrl() {
            var builder = new StringBuilder(Roomful.WebAPIUrl);
            builder.Append(Roomful.Session.Id);
            builder.Append("/");
            builder.Append($"room/{Roomful.RoomService.Room.Template.Id}/");
            builder.Append($"prop/{PropTemplate.Id}/");
            if (ZoomViewMode == ZoomViewModeType.Manage) {
                builder.Append(ZoomViewMode.ToString().ToLower());
            }

            var url = builder.ToString();
            var title = string.IsNullOrEmpty(PropTemplate.Title) ? PropTemplate.Asset.InvokeType.ToString() : PropTemplate.Title;

            return new UrlLink(url, title);
        }
    }
}
