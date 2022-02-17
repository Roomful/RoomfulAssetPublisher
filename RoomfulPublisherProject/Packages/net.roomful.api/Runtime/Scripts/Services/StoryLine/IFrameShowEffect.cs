using System;
using UnityEngine;

namespace net.roomful.api
{
    public interface IFrameShowEffect : IStorylineProgressProvider
    {
        float Duration { get; set; }
        void Play(Action callback, IStoryFrame frame);
        void Pause();
        void Unpause();
        bool IsPlaying { get; }
        Vector3 GetInitialCameraPosition(Camera camera, IStoryFrame frame, bool withAvatar);
    }
}
