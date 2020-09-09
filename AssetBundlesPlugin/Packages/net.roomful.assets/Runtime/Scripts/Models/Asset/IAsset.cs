using UnityEngine;

namespace net.roomful.assets
{
    internal interface IAsset
    {
        Template GetTemplate();
        Texture2D GetIcon();
        void PrepareForUpload();

        GameObject gameObject { get; }
        Component Component { get; }

        bool DrawGizmos { get; set; }
    }
}