using UnityEngine;

namespace net.roomful.api.props
{
    /// <summary>
    /// Read-only prop transform.
    /// </summary>
    public interface IPropTransform
    {
        /// <summary>
        /// Local rotation represented in euler angles.
        /// </summary>
        Vector3 Rotation { get; }

        /// <summary>
        /// Local rotation as quaternion.
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
        /// </summary>
        Bounds NormalizedBounds { get; }

        /// <summary>
        /// Prop Bounds.
        /// </summary>
        Bounds ObjectBounds { get; }

        /// <summary>
        /// Prop transform Forward vector
        /// </summary>
        Vector3 Forward { get; }

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
