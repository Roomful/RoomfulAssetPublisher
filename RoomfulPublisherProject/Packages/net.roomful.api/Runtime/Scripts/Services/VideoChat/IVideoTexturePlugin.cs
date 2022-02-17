using System;
using UnityEngine;

namespace net.roomful.api
{
    public interface IVideoTexturePlugin
    {
        event Action OnTextureSizeUpdated;
        void Init();
        Texture2D PresenterTex { get; }
        Texture2D ShareScreenTex { get; }

        bool IsPresenterTextureReady { get; }
        bool IsShareScreenTextureReady { get; }
    }
}