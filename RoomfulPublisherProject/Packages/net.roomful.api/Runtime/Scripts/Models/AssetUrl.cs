// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    public class AssetUrl
    {
        public string Platform { get; }
        public string Url { get; }

        public AssetUrl(string platform, string url) {
            Platform = platform;
            Url = url;
        }
    }
}