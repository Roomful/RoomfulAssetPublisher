using System;
using System.Collections.Generic;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public interface IProperty
    {
        string Name { get; }
        IReadOnlyDictionary<string, object> Data { get; }
    }
    
    public interface IVideoChatState
    {
        IEnumerable<IProperty> Properties { get; }

        void AddOrUpdateProperty(IProperty property);
        void AddOrUpdateProperties(IEnumerable<IProperty> properties);
        bool HasProperty(string name);
        IProperty GetProperty(string name);
        bool TryGetProperty(string name, out IProperty property);
    }
    
    public interface IVideoChat
    {
        event Action OnChatUpdatedEvent;
        event Action OnReady;
        string Id { get; }
        void Stop();
        IVideoChatStatus Status { get; }
        IVideoChatState State { get; }
    }
}