using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    ///<summary>
    /// Enum for event types. Changing this - change StorylineFrameTimelineEvent.s_Creators, StorylineFrameTimelineEvent.TypeNames
    ///</summary>
    [Serializable]
    public enum EnumStorylineFrameTimelineEventType : int
    {
        AvatarShow = 0,
        AvatarAnimation,
        PlayObjectAnimation,
        FocusOnObject,
        OpenZoomView,
        CloseZoomView,
        ShowRoomUI,
        HideRoomUI,

        COUNT // Should be at the end
    }

    public interface IStorylineFrameTimelineEvent
    {
        ///<summary>
        /// Event type enum.
        ///</summary>
        EnumStorylineFrameTimelineEventType TypeEnum { get; }

        ///<summary>
        /// Event time (timeline absolute value).
        ///</summary>
        float Time { get; set; }

        ///<summary>
        /// Flag indicates that the event must work during the transition.
        ///</summary>
        bool IsWorkInTransition { get; }

        ///<summary>
        /// Event description.
        ///</summary>
        string Description { get; }

        ///<summary>
        /// Flag that the event can be removed from the timeline.
        ///</summary>
        bool CanBeDeleted { get; }
        bool IsAvatarEvent { get; }
        Dictionary<string, object> ToDictionary();
    }
}
