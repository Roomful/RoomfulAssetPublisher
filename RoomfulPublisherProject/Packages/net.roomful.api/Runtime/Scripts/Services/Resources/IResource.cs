using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IResource : ITemplate, ICloneable
    {
        void SetId(string id);
        DateTime LastUpdate { get; }
        string Title { get; set; }
        string Description { get; set; }
        string Category { get; }
        string Location { get; set; }
        string Date { get; set; }
        Dictionary<string, object> Params { get; }
        ContentType Type { get; }
        void UpdateMeta(IResource tpl);
        ResourceMetadata Meta { get; }
        int TotalAvailableReactions { get; set; }
        List<IEmojiReaction> Reactions { get; }
        Emoji Reaction { get; set; }
        string ThumbnailWebURL { get; }
        bool IsEmpty { get; }
        bool IsLocal { get; }
        bool IsDefault { get; }
        EnumThumbnailTag ThumbnailTag { get; set; }
        bool FromTemplate { get; }
        ResourceData ResourceData { get; }
        Dictionary<string, object> ServerParams { get; }
        Dictionary<string, object> ToDictionary();
        T GetParam<T>(ResourceEnum.ParamsKeys paramKey, bool serverParam = false);
        void AddParam(ResourceEnum.ParamsKeys paramKey, object paramValue);
        bool ContainsParam(ResourceEnum.ParamsKeys paramKey, bool serverParam = false);
        IResourceViewInfo ViewInfo { get; }
        void SetTitle(string title);
        void SetDescription(string descr);
        void SetLocationName(string name);
        void SetUserReaction(Emoji reaction);
    }
}
