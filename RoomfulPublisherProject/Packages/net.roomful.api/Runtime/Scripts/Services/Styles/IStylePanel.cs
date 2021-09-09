using UnityEngine;

namespace net.roomful.api.styles
{
    /// <summary>
    /// Style panel representation.
    /// </summary>
    public interface IStylePanel
    {
        Transform Transform { get; }

        /// <summary>
        /// Panel Bounds.
        /// </summary>
        Bounds Bounds { get; }

        /// <summary>
        /// Panel Template
        /// </summary>
        IPanelTemplate Template { get; }

        /// <summary>
        /// Transform roo where all panel graphics is located
        /// </summary>
        Transform GraphicsRoot { get; }

        /// <summary>
        /// Transform roo where all mirrored panel graphics is located
        /// </summary>
        Transform MirrorRoot { get; }
    }
}
