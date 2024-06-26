using System.Collections.Generic;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public interface IRoomTemplate : IRoomContentTemplate, IBaseRoomTemplate
    {
        string Alias { get; }
        string Group { get; }
        IRoomMetadata Meta { get; }
        IReadOnlyList<IStyleTemplate> Styles { get; }
        IResource Preview { get; }
        bool IsPublic { get; }
        IResource Thumbnail { get; }
        float AmbientAudioVolume { get; }
        string AmbientAudioResourceId { get; }
        bool HasOwnReaction { get; }
        int TotalAvailableReactions { get; }
        IReadOnlyList<IUserTemplate> Owners { get; }
        IReadOnlyList<IRoomStory> Stories { get; }
        PermissionTemplate Permissions { get; }
        IReadOnlyList<IRoomStory> PublishedStories { get; }
        IVideoChatSettings VideoChatSettings { get; }
        int BotsCount { get; }
        bool DynamicSocialCirclesAllowed { get; }
        decimal Price { get; }
        
        /// <summary>
        /// True if room can be rented.
        /// When you renting room you are creating room copy for your account.
        /// </summary>
        bool CanBeRented { get; }
        
        /// <summary>
        /// Kind of tags analog for the rooms that can be rented.
        /// </summary>
        IReadOnlyList<string> TemplateStyles { get; }
        JSONData OriginalData { get; }
        JSONData DataWithAssets { get; }
    }
}