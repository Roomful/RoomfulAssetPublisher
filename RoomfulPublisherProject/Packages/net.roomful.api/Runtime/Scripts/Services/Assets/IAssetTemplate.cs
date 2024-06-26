using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.assets
{
    /// <summary>
    /// Base interfaces for the roomful asset template.
    /// </summary>
    public interface IAssetTemplate : ITemplate
    {
        /// <summary>
        /// Template title.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Describes time when asset was created.
        /// </summary>
        DateTime Created  { get; }

        /// <summary>
        /// Describes when asset metadata was updated last time.
        /// </summary>
        DateTime Updated  { get; }

        /// <summary>
        /// Asset associated tags.
        /// </summary>
        IReadOnlyList<string> Tags { get; }

        /// <summary>
        /// Asset bundle download URL for that platform it used on.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Asset Icon resource id.
        /// </summary>
        string IconId  { get; }

        /// <summary>
        /// Get Asset thumbnails.
        /// </summary>
        /// <param name="tag">Custom icon tag for the prop memory management.</param>
        /// <param name="callback">A Callback with the loaded texture.</param>
        void GetThumbnail(EnumThumbnailTag tag, Action<Texture2D> callback);
    }
}
