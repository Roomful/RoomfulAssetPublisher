using net.roomful.api.resources;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// The image resource upload data.
    /// </summary>
    public readonly struct ImageUploadData
    {
        /// <summary>
        /// The name of the new resource.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The type of the new resource.
        /// </summary>
        public readonly string MimeType;

        /// <summary>
        /// New resource data.
        /// </summary>
        public readonly byte[] Data;

        /// <summary>
        /// Origin texture.
        /// Used if Resource type is an image. Otherwise null.
        /// </summary>
        public readonly Texture2D OriginTexture;

        /// <summary>
        /// The resource upload data. Use to create new resource via <see cref="IResourcesService.CreateResource"/>.
        /// </summary>
        /// <param name="name">The name of the new resource.</param>
        /// <param name="mimeType">The type of the new resource.</param>
        /// <param name="data">New resource data.</param>
        public ImageUploadData(string name, string mimeType, byte[] data, Texture2D originTexture) {
            Name = name;
            MimeType = mimeType;
            Data = data;
            OriginTexture = originTexture;
        }
    }
}
