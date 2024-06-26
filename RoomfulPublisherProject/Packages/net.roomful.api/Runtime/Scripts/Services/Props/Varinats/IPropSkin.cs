using System;
using net.roomful.api;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.assets
{
    public interface IPropSkin : ISerializableTemplate {
        string Name { get; }
        DateTime Created { get; }
        DateTime Updated { get; }
        int SortOrder { get; }
        string VariantId { get; set; }
        bool IsDefault { get; }
        void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback);
    }
}
