using UnityEngine;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.lobby
{
    public interface ILobbyElement
    {
        string Id { get; }
        GameObject gameObject { get; }

        void Pause();

        void Resume();
    }
}