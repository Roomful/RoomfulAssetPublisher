
// Copyright Roomful 2013-2018. All rights reserved.



namespace net.roomful.api {
    public interface IStorylineProgressProvider {
        void AddTime(float time);
        float GetElapsedTime();
        void Stop();
    }
}