using UnityEngine;

namespace net.roomful.assets
{
    public interface IAsset
    {

        Template GetTemplate();
        Texture2D GetIcon();
        void PrepareForUpload();


        
        GameObject gameObject { get; }
        Component Component { get; }

        bool DrawGizmos { get; set; }
    }
}

