using UnityEngine;

namespace net.roomful.api.cameras
{
    /// <summary>
    /// The camera behavior.
    /// </summary>
    public interface ICameraBehaviour
    {
        /// <summary>
        /// Set camera target position.
        /// </summary>
        /// <param name="position">new target position.</param>
        /// <param name="skipCorrections">Skip any collision corrections and set exact values.</param>
        void SetTargetPosition(Vector3 position, bool skipCorrections = false);

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
        /// Active camera target rotation.
        /// </summary>
        Vector3 TargetRotation { get; }

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

        /// <summary>
        /// Returns CameraSettings structure
        /// </summary>
        CameraSettings GetCameraSettings();

        /// <summary>
        /// Sets CameraSettings structure
        /// </summary>
        void SetCameraSettings(CameraSettings settings);
        
        float MovementSpeedMultiplier { get; set; }
    }

    /// <summary>
    /// Implement INavigationModeHandler in Camera Behaviour to get keyboard layout mode
    /// </summary>
    public interface IKeyboardLayoutRelatedBehaviour
    {
        bool IsGamerKeyboardLayout { get; }
        void SetGamerKeyboardLayout(bool active);
    }
}
