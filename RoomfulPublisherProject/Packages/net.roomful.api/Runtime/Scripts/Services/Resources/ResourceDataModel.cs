using System;
using System.Collections.Generic;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.api
{
    public class ResourceDataModel : TemplateDataModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ThumbnailWebUrl { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public string LinkId { get; }
        public bool FromTemplate { get; set; }
        public ResourceMetadata Meta { get; set; } = new ResourceMetadata();
        public DateTime LastUpdate { get; set; } = DateTime.MinValue;
        public string LastUpdateRfc3339 => RoomfulTime.DateTimeToRfc3339(LastUpdate);
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, object> ServerParams { get; set; } = new Dictionary<string, object>();
        public ContentStatus Status { get; set; }
        public IResourceViewInfo ViewInfo { get; set; } = new ResourceViewInfo();
        public ResourceData ResourceData { get; set; }
        public int TotalAvailableReactions { get; set; }
        public List<EmojiReaction> Reactions { get; } = new List<EmojiReaction>();
        public Emoji OwnReaction { get; set; } = Emoji.none;

        public ResourceDataModel() { }

        public ResourceDataModel(JSONData resourceInfo) : base(resourceInfo) {

            if (Id.Equals("05bb047vdqhrf9")) {
                Debug.LogError($"tm log: {resourceInfo.GetValue<string>("thumbnail")}");
            }
            if (resourceInfo.HasValue("updated")) {
                LastUpdate = resourceInfo.GetValue<DateTime>("updated");
            }

            Title = resourceInfo.GetValue<string>("title");
            Description = resourceInfo.GetValue<string>("description");
            if (resourceInfo.HasValue("location")) {
                Location = resourceInfo.GetValue<string>("location");
            }

            if (resourceInfo.HasValue("date")) {
                Date = resourceInfo.GetValue<string>("date");
            }

            if (resourceInfo.HasValue("category")) {
                Category = resourceInfo.GetValue<string>("category");
            }

            if (resourceInfo.HasValue("thumbnail")) {
                ThumbnailWebUrl = resourceInfo.GetValue<string>("thumbnail");
            }

            if (resourceInfo.HasValue("status")) {
                var status = resourceInfo.GetValue<string>("status");
                switch (status) {
                    case "pending":
                        Status = ContentStatus.Pending;
                        break;
                    case "processing":
                        Status = ContentStatus.Processing;
                        break;
                    case "ready":
                        Status = ContentStatus.Ready;
                        break;
                    case "failed":
                        Status = ContentStatus.Failed;
                        break;
                }
            }

            if (resourceInfo.HasValue("view")) {
                var vInfo = new JSONData(resourceInfo.GetValue<Dictionary<string, object>>("view"));
                ViewInfo = new ResourceViewInfo(vInfo);
            }

            var jsonMetaInfo = new JSONData(resourceInfo.GetValue<Dictionary<string, object>>("metadata"));
            Meta = new ResourceMetadata(jsonMetaInfo);
            if (resourceInfo.HasValue("params")) {
                var paramsString = resourceInfo.GetValue<string>("params");
                if (!string.IsNullOrEmpty(paramsString)) {
                    var paramsData = new JSONData(paramsString);
                    foreach (var param in paramsData.Data) {
                        Params.Add(param.Key, param.Value);
                    }
                }
            }

            if (resourceInfo.HasValue("data")) {
                //TODO replace server params and params with structure key, class(amazon, youtube, imdb, etc.)
                ServerParams = resourceInfo.GetValue<Dictionary<string, object>>("data");
                ResourceData = new ResourceData(ServerParams, Params);
            }

            if (resourceInfo.HasValue("isDefault")) {
                IsDefault = resourceInfo.GetValue<bool>("isDefault");
            }

            if (resourceInfo.HasValue("linkId")) {
                LinkId = resourceInfo.GetValue<string>("linkId");
            }

            if (resourceInfo.HasValue("fromTemplate")) {
                FromTemplate = resourceInfo.GetValue<bool>("fromTemplate");
            }

            if (resourceInfo.HasValue("reactions")) {
                var reactionsInfo = new JSONData(resourceInfo.GetValue<Dictionary<string, object>>("reactions"));
                TotalAvailableReactions = reactionsInfo.GetValue<int>("total");
                var reactions = reactionsInfo.GetValue<List<object>>("list");
                if (reactions == null) {
                    reactions = new List<object>();
                }

                foreach (var reaction in reactions) {
                    var jsonCommentInfo = new JSONData(reaction);
                    var c = new EmojiReaction(jsonCommentInfo);
                    Reactions.Add(c);
                }

                if (reactionsInfo.HasValue("ownReaction")) {
                    var emojiString = reactionsInfo.GetValue<string>("ownReaction");
                    OwnReaction = string.IsNullOrEmpty(emojiString)
                        ? Emoji.none
                        : EnumUtility.ParseEnum<Emoji>(emojiString);
                }
            }
        }

        public bool ContainsParam(string paramKey) {
            return Params.ContainsKey(paramKey) && Params[paramKey] != null;
        }

        public object GetParam(string paramKey) {
            object valueByKey = null;
            var param = paramKey.ToLower();
            if (Params.ContainsKey(param)) {
                valueByKey = Params[param];
            }

            return valueByKey;
        }

        public void SetParam(string paramKey, object paramValue) {
            var param = paramKey.ToLower();
            if (Params.ContainsKey(param)) {
                Params[param] = paramValue;
            }
            else {
                Params.Add(param, paramValue);
            }
        }

        public void RemoveParam(string paramKey) {
            var param = paramKey.ToLower();
            if (Params.ContainsKey(param)) {
                Params.Remove(param);
            }
        }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            data.Add("title", Title);
            data.Add("description", Description);
            data.Add("location", Location);
            data.Add("date", Date);
            data.Add("updated", LastUpdateRfc3339);
            if (!string.IsNullOrEmpty(Category)) {
                data.Add("category", Category);
            }

            if (!ThumbnailWebUrl.Equals(string.Empty)) {
                data.Add("thumbnail", ThumbnailWebUrl);
            }

            data.Add("metadata", Meta.ToDictionary());
            data.Add("view", ViewInfo.ToDictionary());
            data.Add("data", ServerParams);
            data.Add("params", Json.Serialize(Params));

            var reactionsInfo = new Dictionary<string, object>();
            var reactionsList = new List<object>();
            foreach (var reactions in Reactions) {
                reactionsList.Add(reactions.ToDictionary());
            }

            reactionsInfo.Add("total", TotalAvailableReactions);
            reactionsInfo.Add("list", reactionsList);
            reactionsInfo.Add("ownReaction", OwnReaction.ToString());
            data.Add("reactions", reactionsInfo);

            return data;
        }

        public ContentType Type {
            get {
                if (Meta.MimeType == ResourceMetadata.MimeTypeGif) {
                    return ContentType.Gif;
                }
                if (ResourceMetadata.IMAGE_TYPES.Contains(Meta.MimeType)) {
                    return ContentType.Image;
                }

                if (ResourceMetadata.VIDEO_TYPES.Contains(Meta.MimeType)) {
                    return ContentType.Video;
                }

                if (ResourceMetadata.AUDIO_TYPES.Contains(Meta.MimeType)) {
                    return ContentType.Audio;
                }

                if (ResourceMetadata.BOOK_TYPES.Contains(Meta.MimeType)) {
                    return ContentType.Book;
                }

                return ResourceMetadata.SPECIAL_TYPES.Contains(Meta.MimeType) ? ContentType.Directory : ContentType.Undefined;
            }
        }
    }
}
