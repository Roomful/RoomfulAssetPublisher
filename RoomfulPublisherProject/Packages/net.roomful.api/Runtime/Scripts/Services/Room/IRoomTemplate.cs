﻿using System;
using System.Collections.Generic;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public interface IRoomTemplate : IRoomContentTemplate
    {
        string Alias { get; }
        string Group { get; }
        IRoomMetadata Meta { get; }
        IReadOnlyList<IStyleTemplate> Styles { get; }
        IResource Preview { get; }
        bool HasPreview { get; }
        bool IsPublic { get; }
        IResource Thumbnail { get; }
        float AmbientAudioVolume { get; }
        string AmbientAudioResourceId { get; }
        bool HasOwnReaction { get; }
        int TotalAvailableReactions { get; }
        IEnvironmentAssetTemplate Environment { get; }
        IReadOnlyList<IUserTemplate> Owners { get; }
        IReadOnlyList<IRoomStory> Stories { get; }
        PermissionTemplate Permissions { get; }
        IReadOnlyList<IRoomStory> PublishedStories { get; }
        IVideoChatSettings VideoChatSettings { get; }
        int BotsCount { get; }
        void GetThumbnail(Action<Texture2D> callback);
        void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback);
        bool DynamicSocialCirclesAllowed { get; }
        JSONData OriginalData { get; }
    }
}