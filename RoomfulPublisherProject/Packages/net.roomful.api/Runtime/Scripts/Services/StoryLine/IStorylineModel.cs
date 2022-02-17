using System;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IStorylineModel {
        IRoomStory Story { get; set; }
        IStoryFrame ActiveFrame { get; set; }
        int ActiveFrameIndex { get; }
        StoryRecordOptions EditorPlaybackOptions { get; }
        event Action<IStoryFrame> OnActiveFrameUpdated;
        event Action<IStoryFrame> OnFrameUpdated;
        event Action<IRoomStory> OnStoryUpdated;
        event Action OnStoryDeleted;
        void InitStory();
        void FreeStory();
        void RemoveStory();
        void UpdateActiveFrame();
        void UpdateFrame(IStoryFrame frame);
    }
}
