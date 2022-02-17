using System;
using net.roomful.api.props;
using net.roomful.api.styles;
using UnityEngine;

namespace net.roomful.api.cameras
{
    /// <summary>
    /// Camera rey cas hits
    /// </summary>
    public struct CameraRayHits
    {
        /// <summary>
        /// Number of the hits
        /// </summary>
        public int Size;

        /// <summary>
        /// Hits array. Max 10.
        /// </summary>
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

        /// <summary>
        /// Fired when camera behaviour is changed.
        /// </summary>
        event Action OnCameraBehaviourChanged;

        /// <summary>
        /// Fired when camera has moved to the room default point.
        /// </summary>
        event Action OnCameraMovedToTheRoomDefaultPoint;

        /// <summary>
        /// Fired when camera has moved to the room default point.
        /// </summary>
        event Action<RoomCameraPoint> OnCameraAboutToMoveToTheRoomDefaultPoint;

        /// <summary>
        /// Event is fired when camera moved with certain tolerance.
        /// </summary>
        event Action OnMoveDiscretely;

        /// <summary>
        /// Fired when camera resolution is hanged.
        /// </summary>
        event Action OnCameraResolutionUpdated;

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

        /// <summary>
        /// Instance of the World Space UI camera.
        /// </summary>
        Camera WorldSpaceUICamera { get; }

        Vector2Int CameraResolution { get; }

        /// <summary>
        /// Gives an ability to override room default point.
        /// </summary>
        /// <param name="callback">Action to define default room point.</param>
        void OverrideGetRoomDefaultCameraPoint(Action<Action<RoomCameraPoint>> callback);

        /// <summary>
        /// Move camera to the default room point.
        /// </summary>
        void GoToDefaultRoomPoint();

        /// <summary>
        /// Move camera to the default room point instantly.
        /// </summary>
        void GoToDefaultRoomPointImmediate();

        /// <summary>
        /// Fly to specific panel.
        /// </summary>
        /// <param name="stylePanel">Target panel.</param>
        /// <param name="onComplete">Fired when camera fly is completed.</param>
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

        /// <summary>
        /// Set new camera behaviour
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>New camera behaviour instance.</returns>
        T SetCameraBehaviour<T>() where T : ICameraBehaviour;

        /// <summary>
        /// Get camera raycast hits.
        /// </summary>
        /// <param name="point">Screen point for raycast.</param>
        /// <param name="distance">Raycast distance.</param>
        /// <returns><see cref="CameraRayHits"/> info model.</returns>
        CameraRayHits GetRaycastHits(Vector2 point, float distance = Mathf.Infinity);

        /// <summary>
        /// Get camera raycast hits.
        /// </summary>
        /// <param name="point">Screen point for raycast.</param>
        /// <param name="layerMask">Layer mask for raycast.</param>
        /// <param name="distance">Raycast distance.</param>
        /// <returns><see cref="CameraRayHits"/> info model.</returns>
        CameraRayHits GetRaycastHits(Vector2 point, int layerMask, float distance = Mathf.Infinity);

        /// <summary>
        /// Get camera raycast hits.
        /// </summary>
        /// <param name="fromPosition">Raycast start position.</param>
        /// <param name="toPosition">Raycast end position.</param>
        CameraRayHits GetRaycastHits(Vector3 fromPosition, Vector3 toPosition);

        /// <summary>
        /// Converts screen point to ray.
        /// </summary>
        /// <param name="point">Screen point.</param>
        /// <returns>Ray based on provided screen point.</returns>
        Ray ScreenPointToRay(Vector2 point);

        /// <summary>
        /// Sets camera behaviour to default.
        /// </summary>
        void SetDefaultBehaviour();

        /// <summary>
        /// Disables camera rendering.
        /// </summary>
        void DisableCamera();

        /// <summary>
        /// Enables camera rendering.
        /// </summary>
        void EnableCamera();
    }
}
