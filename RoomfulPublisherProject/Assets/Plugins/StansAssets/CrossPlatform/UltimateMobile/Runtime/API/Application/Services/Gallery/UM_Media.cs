using UnityEngine;

namespace SA.CrossPlatform.App
{
    /// <summary>
    /// Picked image from gallery representation
    /// </summary>
    public class UM_Media
    {

        /// <summary>
        /// Device local path to the current media file.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; }

        /// <summary>
        /// Media file thumbnail.
        /// </summary>
        public Texture2D Thumbnail { get; }

        /// <summary>
        /// Type of yhe media
        /// </summary>
        public UM_MediaType Type { get; }

        public UM_Media(Texture2D thumbnail, string path, UM_MediaType type)
        {
            Path = path;
            Type = type;
            Thumbnail = thumbnail;
        }
    }
}
