using System;
using System.Collections.Generic;
using net.roomful.api.assets;

namespace net.roomful.api
{
    public interface IResource : ITemplate
    {
        DateTime LastUpdate { get; }
        string Title { get; }
        string Description { get; }
        string Category { get; }
        string Location { get; }
        string Date { get; }
        Dictionary<string, object> Params { get; }
        ContentStatus Status { get; }
        ContentType Type { get; }
        string ConvertedFromType { get; }
        // void UpdateMeta(IResource tpl);
        ResourceMetadata Meta { get; }
        ResourceData Data { get; }
        int TotalAvailableReactions { get;}
        IReadOnlyList<IEmojiReaction> Reactions { get; }
        Emoji OwnReaction { get; }
        string ThumbnailWebURL { get; }

        /// <summary>
        /// If this resource was created like a link we will have
        /// id of the original resource in this property.
        /// </summary>
        string LinkId { get; }
        EnumThumbnailTag ThumbnailTag { get; set; }
        bool FromTemplate { get; }
        Dictionary<string, object> ToDictionary();
        T GetParam<T>(ResourceEnum.ParamsKeys paramKey, bool serverParam = false);
        void AddParam(ResourceEnum.ParamsKeys paramKey, object paramValue);
        bool ContainsParam(ResourceEnum.ParamsKeys paramKey);

        T GetParam<T>(string paramKey, bool serverParam = false);
        void AddParam(string paramKey, object paramValue);
        bool ContainsParam(string paramKey);
        
        bool ContainsServerParam(string paramKey);
    }
}
