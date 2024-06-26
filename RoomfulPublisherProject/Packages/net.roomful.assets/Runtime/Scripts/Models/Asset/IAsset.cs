using UnityEngine;

namespace net.roomful.assets
{
    internal interface IAsset : IAssetBundle
    {
        AssetTemplate GetTemplate();
        Texture2D GetIcon();
        void PrepareForUpload();
        Component Component { get; }
        bool DrawGizmos { get; set; }
    }
}