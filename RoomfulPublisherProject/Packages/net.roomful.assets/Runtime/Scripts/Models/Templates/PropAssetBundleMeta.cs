using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.assets
{
    internal class PropAssetBundleMeta
    {
        /// <summary>
        /// show amount of logos in content management system.
        /// </summary>
        public int LogoCount { get; set; }

        /// <summary>
        /// show amount of thumbnails in content management system
        /// </summary>
        public int ThumbnailCount { get; set; }

        public PropAssetBundleMeta() { }

        public PropAssetBundleMeta(JSONData assetData) {
            var assetBundleMeta = new JSONData(assetData.GetValue<Dictionary<string, object>>("assetBundleMeta"));
            LogoCount = assetBundleMeta.GetValue<int>("logoCount");
            ThumbnailCount = assetBundleMeta.GetValue<int>("thumbnailCount");
        }

        public void AppendDictionary(Dictionary<string, object> data) {
            var assetBundleMeta = new Dictionary<string, object> {
                { "logoCount", LogoCount },
                { "thumbnailCount", ThumbnailCount }
            };
            data.Add("assetBundleMeta", assetBundleMeta);
        }
    }
}
