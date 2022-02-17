using System;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public interface IVideoChat
    {
        event Action OnChatUpdatedEvent;
        event Action OnReady;
        string Id { get; }
        void Stop();
        IVideoChatStatus Status { get; }
    }
}