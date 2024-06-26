using System;
using UnityEngine;
using UnityEngine.Video;

namespace net.roomful.api.player.video
{
    public enum VisibilityScope
    {
        Singleton,
        Transient
    }
    
    public interface IVideoPlayerService
    {
        void ChangeVideoPlayerState(Renderer renderer, VideoPlayer player);

        IVideoPlayer GetPlayer(VisibilityScope scope);
        void GetVideoUrlForResource(IResource resource, Action<string> callback);
    }
}