using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Read-only prop transform.
    /// </summary>
    public interface IPropTransform
    {
        /// <summary>
        /// The parent of the prop transform.
        /// </summary>
        IPropTransform Parent { get; }

        /// <summary>
        /// Local rotation represented in euler angles.
        /// </summary>
        Vector3 Rotation { get; }

        /// <summary>
        /// Global rotation represented in euler angles.
        /// </summary>
        Vector3 GlobalRotation { get; }

        /// <summary>
        /// Global rotation as quaternion.
        /// </summary>
        Quaternion RotationQuaternion { get; }

        /// <summary>
        /// Local rotation represented in euler angles.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Prop scale.
        /// </summary>
        float Scale { get; }

        /// <summary>
        /// Prop bounds without rotation consideration.
        /// Use Prop Inspector to visualize. In most cases you want <see cref="ObjectBounds"/>.
        /// </summary>
        Bounds NormalizedBounds { get; }

        /// <summary>
        /// Prop Bounds.
        /// Use Prop Inspector to visualize.
        /// </summary>
        Bounds ObjectBounds { get; }

        /// <summary>
        /// Bounds calculated from Renderers that prop contains, including all children.
        /// Please use this method efficiently, because it's quite heavy.
        /// </summary>
        Bounds RendererBounds { get; }

        /// <summary>
        /// RendererBounds without rotation consideration.
        /// </summary>
        Bounds NormalizedRendererBounds { get; }

        /// <summary>
        /// Prop transform Forward vector
        /// </summary>
        Vector3 Forward { get; }
        
        /// <summary>
        /// Prop transform Right vector
        /// </summary>
        Vector3 Right { get; }

        /// <summary>
        /// Bottom Center Vertex point.
        /// </summary>
        Vector3 BottomCenterVertex { get; }

        /// <summary>
        /// Back Center Vertex point.
        /// </summary>
        Vector3 BackCenterVertex { get; }

        /// <summary>
        /// Front Center Vertex point.
        /// </summary>
        Vector3 FrontCenterVertex { get; }

        /// <summary>
        /// Front Upper Center Vertex point.
        /// </summary>
        Vector3 FrontUpperCenterVertex { get; }

        /// <summary>
        /// Transforms position from world space to local space.
        /// Fore more info, check Unity documentation here:
        /// https: //docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
        /// </summary>
        /// <param name="position">The position to transform.</param>
        Vector3 InverseTransformPoint(Vector3 position);

        /// <summary>
        /// Transforms position from local space to world space.
        /// Fore more info, check Unity documentation here:
        /// https: //docs.unity3d.com/ScriptReference/Transform.TransformPoint.html
        /// </summary>
        /// <param name="position">The position to transform.</param>
        Vector3 TransformPoint(Vector3 position);
    }
}
