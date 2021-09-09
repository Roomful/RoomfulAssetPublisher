using UnityEngine;

namespace net.roomful.api.cameras
{
    public interface ICameraBehaviour
    {
        /// <summary>
        /// Set camera target position.
        /// </summary>
        /// <param name="position">new target position.</param>
        void SetTargetPosition(Vector3 position);

        /// <summary>
        /// Set camera target rotation.
        /// </summary>
        /// <param name="rotation">new target rotation.</param>
        void SetTargetRotation(Vector3 rotation);

        /// <summary>
        /// Active camera target position.
        /// </summary>
        Vector3 TargetPosition { get; }

        /// <summary>
        /// Change the state of a camera behaviour. True means that behaviour logic is active.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Active camera position.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Active camera rotation.
        /// </summary>
        Vector3 Rotation { get; }
    }
}
