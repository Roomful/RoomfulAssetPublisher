using System;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets {
    
    public interface IPropVariant : ISerializableTemplate {
        string Name { get; }
        DateTime Created { get; }
        DateTime Updated { get; }
        void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback);
    }
}