using UnityEngine;

namespace RF.AssetBundles.Serialization {

    public interface IVideoTexturePlugin {
        void Init();

        Texture2D PresenterTex { get; }
        Texture2D ShareScreenTex { get; }

        bool IsPresenterTextureReady { get; }
        bool IsShareScreenTextureReady { get; }
    }
}
