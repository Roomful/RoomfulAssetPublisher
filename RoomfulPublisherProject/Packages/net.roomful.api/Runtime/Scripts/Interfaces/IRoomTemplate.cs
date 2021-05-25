using System;
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
        IReadOnlyList<IEmojiReaction> Reactions { get; }
        bool HasOwnReaction { get; }
        IEnvironmentAssetTemplate Environment { get; }
        IReadOnlyList<IUserTemplate> Owners { get; }
        IReadOnlyList<IRoomStory> Stories { get; }
        PermissionTemplate Permissions { get; }
        IReadOnlyList<IRoomStory> PublishedStories { get; }
        IVideoChatSettings VideoChatSettings { get; }
        IAvatarsLocationSettings AvatarsLocationSettings { get; }
        int BotsCount { get; }
        void GetThumbnail(Action<Texture2D> callback);
        void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback);
        bool DynamicSocialCirclesAllowed { get; }

        IReadOnlyCollection<IRoomPoint> RoomPoints { get; }
        void AddRoomPoint(IRoomPoint point);
        void RemoveRoomPoint(IRoomPoint point);
    }
}
