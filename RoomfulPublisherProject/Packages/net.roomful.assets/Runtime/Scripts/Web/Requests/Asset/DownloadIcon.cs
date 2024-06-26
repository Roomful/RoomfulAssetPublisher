using System.Collections.Generic;

namespace net.roomful.assets
{
    internal class DownloadIcon : BaseWebPackage
    {
        private const RequestMethods PackMethodName = RequestMethods.GET;

        public DownloadIcon(string url) : base(url, PackMethodName) { }

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            return originalJSON;
        }

        public override bool IsDataPack => true;
    }
}