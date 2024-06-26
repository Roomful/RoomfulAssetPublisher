using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// Style asset template.
    /// </summary>
    public interface IStyleAssetTemplate : IAssetTemplate
    {
        /// <summary>
        /// Style asset metadata.
        /// </summary>
        StyleAssetMetadata Metadata { get; }

        /// <summary>
        /// Default camera position for the style.
        /// It will only be applied if the style is a first style in the room.
        /// </summary>
        Vector3 HomePosition { get; }

        /// <summary>
        /// Defines style type.
        /// </summary>
        StyleType StyleType { get; }

        /// <summary>
        /// Style doors type. Please note that this will be applied to the whole room.
        /// Style has to be the first style in the room for type to take the effect.
        /// </summary>
        StyleDoorsType DoorsType { get; }
    }
}
