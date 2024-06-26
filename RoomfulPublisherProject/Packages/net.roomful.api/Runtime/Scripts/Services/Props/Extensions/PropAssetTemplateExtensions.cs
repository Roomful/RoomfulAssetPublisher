using System.Linq;
using net.roomful.api;

namespace net.roomful.api.props
{
    public static class PropAssetTemplateExtensions
    {
        public static string FAKE_PROP_TAG = "fakeProp";
        
        public static bool IsFakeProp(this IPropAssetTemplate @this) {
            return @this.Tags.Contains(FAKE_PROP_TAG);
        }
    }
}