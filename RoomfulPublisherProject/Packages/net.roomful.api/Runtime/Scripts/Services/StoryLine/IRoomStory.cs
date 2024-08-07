﻿using System;
using System.Collections.Generic;
using net.roomful.api.audio;

namespace net.roomful.api {

    public interface IRoomStory : IRoomContentTemplate {

        Action<IStoryFrame> OnFrameRemoved { get; set; }
        Action<List<IStoryFrame>, int> OnFramesAdded  { get; set; }
        Action<IStoryFrame, int, int> OnFrameMoved { get; set; }
        Action OnMetaUpdated  { get; set; }

        bool Published { get; }
        string Title { get; }
        DateTime Created { get; set; }
        List<IStoryFrame> Frames { get; }
        AudioResource BackgroundMusic { get; }
        bool BackgroundMusicAvaliable { get; }
        void SetBackgroundMusic(AudioResource music);
        List<IStoryFrame> GetValidFrames();
        float GetBackgroundMusicVolume();
        void RemoveFrame(IStoryFrame frame);
        void SetTitle(string value);
        void AddFrame(IStoryFrame frame, int pos);
        void SetPublish(bool published);
        void MoveFrame(IStoryFrame frame, int newPos);
        void AddFrames(List<IStoryFrame> frames, int pos);
        string AvatarId { get; set; }
        float LipSyncSensitivity { get; set; }

        /// <summary>
        /// The prop id story line is bound to.
        /// If value is empty it means that storyline is bound to the room.
        /// </summary>
        string PropId { get; }
        string RoomId { get; }
    }
}
