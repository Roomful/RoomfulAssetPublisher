using UnityEngine;

namespace net.roomful.api.styles
{
    /// <summary>
    /// Style panel representation.
    /// </summary>
    public interface IStylePanel
    {
        /// <summary>
        /// Panel Icon.
        /// </summary>
        Texture2D Icon { get; }

        /// <summary>
        /// Indicates if this is the first panel of the room.
        /// </summary>
        bool IsStartPanel { get; }
        
        /// <summary>
        /// Indicates if this is the last panel in the room.
        /// </summary>
        bool IsEndPanel{ get; }
        
        /// <summary>
        /// Panel transform.
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        /// Transform for parting props and other panel related objects.
        /// </summary>
        Transform FloorPropsRoot { get; }

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

        /// <summary>
        /// Transforms 'position' from local space to world space.
        /// Note that the returned position is affected by scale. Use Transform.TransformDirection if you are dealing with direction vectors.
        /// </summary>
        /// <param name="position">The position to transform.</param>
        /// <returns>Transformed position.</returns>
        Vector3 TransformPoint(Vector3 position);
    }
}
