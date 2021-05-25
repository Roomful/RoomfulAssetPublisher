using System.Linq;

namespace net.roomful.api.props
{
    // ReSharper disable once InconsistentNaming
    public static class IPropAssetExtensions
    {
        public static bool CanHoldContent(this IPropAssetTemplate propAssetTemplate) {
            return propAssetTemplate.ContentTypes.Any();
        }
    }
}
