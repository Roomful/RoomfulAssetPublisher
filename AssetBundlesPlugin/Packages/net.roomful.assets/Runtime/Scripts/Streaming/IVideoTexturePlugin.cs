using UnityEngine;

namespace net.roomful.assets.serialization
{
    internal interface IVideoTexturePlugin
    {
        void Init();

        Texture2D PresenterTex { get; }
        Texture2D ShareScreenTex { get; }

        bool IsPresenterTextureReady { get; }
        bool IsShareScreenTextureReady { get; }
    }
}