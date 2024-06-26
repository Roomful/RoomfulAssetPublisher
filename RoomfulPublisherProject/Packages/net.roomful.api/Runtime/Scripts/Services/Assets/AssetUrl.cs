// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.assets
{
    /// <summary>
    /// Url to the asset.
    /// </summary>
    public class AssetUrl
    {
        /// <summary>
        /// Asset platform.
        /// </summary>
        public string Platform { get; }

        /// <summary>
        /// Url string.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Creates new asset url.
        /// </summary>
        /// <param name="platform">Asset platform.</param>
        /// <param name="url">Download url.</param>
        public AssetUrl(string platform, string url) {
            Platform = platform;
            Url = url;
        }
    }
}
