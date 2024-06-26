using System;
using net.roomful.api;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.assets {

    public interface IPropVariant : ISerializableTemplate {
        string Name { get; }
        int SortOrder { get; }
        DateTime Created { get; }
        DateTime Updated { get; }
        void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback);
    }
}
