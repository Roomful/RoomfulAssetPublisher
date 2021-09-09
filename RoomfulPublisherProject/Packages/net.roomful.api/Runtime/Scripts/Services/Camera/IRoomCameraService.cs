using System;
using net.roomful.api.props;
using net.roomful.api.styles;
using UnityEngine;

namespace net.roomful.api.cameras
{
    public struct CameraRayHits
    {
        public int Size;
        public RaycastHit[] Hits;
    }

    /// <summary>
    ///  An Accesses point to the Room camera.
    /// </summary>
    public interface IRoomCameraService
    {
        /// <summary>
        /// Fires when camera snaps
        /// </summary>
        event Action<TurnDirection> OnCameraSnapped;

        event Action OnCameraBehaviourChanged;

        /// <summary>
        /// Main room camera.
        /// </summary>
        Camera MainCamera { get; }

        /// <summary>
        /// Quick access to the <see cref="MainCamera"/> transform component.
        /// </summary>
        Transform MainCameraTransform { get; }

        /// <summary>
        /// Represents current camera behaviour
        /// </summary>
        ICameraBehaviour Behaviour { get; }

        /// <summary>
        /// Type of default Camera Behaviour
        /// </summary>
        Type DefaultCameraBehaviour { get; }

        Camera WorldSpaceUICamera { get; }

        /// <summary>
        /// Event is fired when camera moved with certain tolerance.
        /// </summary>
        event Action OnMoveDiscretely;

        void FlyTo(IStylePanel stylePanel, Action onComplete = null);

        /// <summary>
        /// Fly Camera to the prop.
        /// described by provided position and rotation.
        ///
        /// </summary>
        /// <param name="prop">prop to fly to</param>
        /// <param name="onComplete">Action will be triggered once camera will reach target destination.</param>
        /// <param name="stopPreviousTransition">Set as true to stop previous FlyTo transition, otherwise FlyTo will no be executed.</param>
        void FlyTo(IProp prop, Action onComplete = null, bool stopPreviousTransition = false);


        /// <summary>
        /// Camera will perform cinematic fly from current point to the new point in space
        /// described by provided position and rotation.
        ///
        /// </summary>
        /// <param name="position">Target point position.</param>
        /// <param name="rotation">Target point rotation.</param>
        /// <param name="onComplete">Action will be triggered once camera will reach target destination.</param>
        /// <param name="stopPreviousTransition">Set as true to stop previous FlyTo transition, otherwise FlyTo will no be executed.</param>
        void FlyTo(Vector3 position, Vector3 rotation, Action onComplete = null, bool stopPreviousTransition = false);

        /// <summary>
        /// Stops camera fly if it is active at this moment.
        /// </summary>
        void StopFlyTo();

        /// <summary>
        /// Method performs requests and returns first prop located at provided screen coords
        /// </summary>
        /// <param name="position">Screen position.</param>
        /// <returns>Prop instance if found, otherwise `null`.</returns>
        IProp GetPropAtScreenPosition(Vector2 position);

        T SetCameraBehaviour<T>() where T : ICameraBehaviour;

        /// <summary>
        /// Set default Camera behaviour
        /// </summary>
        void SetDefaultBehaviour();

        CameraRayHits GetRaycastHits(Vector2 point, float distance = Mathf.Infinity);

        Ray ScreenPointToRay(Vector2 point);
    }
}
