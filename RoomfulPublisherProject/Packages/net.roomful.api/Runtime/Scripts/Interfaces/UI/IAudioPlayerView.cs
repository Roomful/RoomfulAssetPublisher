using System;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public interface IAudioPlayerView {
        
        event Action OnPlayClicked;
        void SetProgress(float progress, float minutes, float seconds);
        void SetLength(float minutes, float seconds);
        void SetPlayButtonState(bool isPlaying);
    }
}
