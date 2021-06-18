using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public interface IStoryFrame
    {
        bool IsValid { get; }
        Vector3 FocusPosition { get; }
        Vector3 PropPosition { get; }
        void Init();
        void Free();
        void GetIcon(Action<Texture2D> texture);
        int FrameId { get; set; }
        string NavigationTitle { get;}
        float AudioNarrationDuration { get; set; }
        float CalculatedDuration { get; set; }
        string Type { get; }
        IResource AudioNarration { get; }
        bool HasNarration { get; }
        float DurationWithTransition { get; }
        float PreferredDuration { get; }
        bool CanBeShownInZoomView { get; }
        int TotalComments { get; }
        bool ContainsVideo { get; }
        bool SuppressBackgroundMusic { get; }
        bool HasCommentsToPlay { get; }
        float CommentsShowTime { get; }
        IFrameShowEffect ShowEffect { get; set; }
        FrameTransition Transition { get; set;}
        Dictionary<string, object> ToDictionary();
        void SetAudioNarration(IResource resource);
        void SetPreferredDuration(float duration);
        void SetNavigationTitle(string text);
        Vector3 FocusPointer { get; }
        Vector3 BoundsSize { get; }
        Bounds NormalizedBounds { get; }
        Vector3 Rotation { get; }
        void PrepareForUse();
        void CollectResourcesForDownload(List<IResource> downloadRes);
        void InitTimeline();
        IStorylineFrameTimeline Timeline { get; set; }
        PlacingType Placing { get; }
        Bounds RendererBounds { get; }
        Vector3 Forward { get; }
        void SetDirty();
    }
}