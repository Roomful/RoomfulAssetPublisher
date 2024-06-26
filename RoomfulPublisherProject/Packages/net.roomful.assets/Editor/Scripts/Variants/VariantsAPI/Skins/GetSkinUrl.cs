using System.Collections.Generic;

namespace net.roomful.assets.editor
{
    internal class GetSkinUrl : BaseWebPackage
    {
        private const string PACK_URL = "/api/v0/asset/skin/url/";

        private const RequestMethods PackMethodName = RequestMethods.GET;

        public GetSkinUrl(string skinId, string platform) : base(PACK_URL, PackMethodName) {
            AddToUrl($"{skinId}/platform/{platform}");
        }

        public override Dictionary<string, object> GenerateData() {
            var originalJson = new Dictionary<string, object>();
            return originalJson;
        }
    }
}