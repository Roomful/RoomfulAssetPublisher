using System;

namespace net.roomful.assets
{
    [Serializable]
    internal class EnvironmentAssetTemplate : AssetTemplate
    {
        public EnvironmentAssetTemplate() : base() { }
        public EnvironmentAssetTemplate(string data) : base(data) { }
    }
}