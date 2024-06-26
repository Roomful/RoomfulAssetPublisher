using UnityEngine;

namespace net.roomful.assets
{
    internal interface IAssetBundle
    {
        string Title { get; }
        GameObject gameObject { get; }
    }
}