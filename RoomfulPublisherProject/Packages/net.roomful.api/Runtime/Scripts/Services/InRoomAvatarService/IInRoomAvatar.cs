using System;
using System.Collections.Generic;
using net.roomful.api.props;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    ///<summary>
    /// In room avatar interface
    ///</summary>
    public interface IInRoomAvatar
    {
        /// <summary>
        /// Asset ID of the avatar
        /// </summary>
        string AssetId { get; }

        /// <summary>
        /// User Id avatar is bound to.
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// Avatar prop.
        /// </summary>
        IProp Prop { get; }

        ///<summary>
        /// Flag that the avatar is shown
        ///</summary>
        bool Enabled { get; set; }

        //IInRoomAvatarUI_NEw AvatarUI { get; }

        ///<summary>
        /// Scene object of the avatar component
        ///</summary>
        GameObject GameObject { get; }

        /// <summary>
        /// Notifies about the avatar has touched
        /// </summary>
        event Action<IInRoomAvatar> OnTouched;

        ///<summary>
        /// Dictionary of avatar animations names-lengths
        ///</summary>
        IReadOnlyDictionary<string, float> Animations { get; }

        ///<summary>
        /// Return bounds of the avatar's game object
        ///</summary>
        Bounds GetRendererBounds();

        ///<summary>
        /// Set moving speed of the avatar
        ///</summary>
        void SetSpeed(float speed);

        ///<summary>
        /// Set stopping distance for destination point of the avatar
        ///</summary>
        void SetStoppingDistance(float distance);

        ///<summary>
        /// Set radius of the avatar
        ///</summary>
        void SetRadius(float radius);

        ///<summary>
        /// When the avatars is performing avoidance, avatars of lower priority are ignored.
        /// The valid range is from 0 to 99 where: Most important = 0. Least important = 99. Default = 50.
        ///</summary>
        void SetAvoidancePriority(int priority);

        ///<summary>
        /// Teleport avatar at given position
        ///</summary>
        ///<param name="pos">Position to teleport</param>
        /// <param name="callback">Callback when moving complete. Flag tells whether the destination has been reached</param>
        void Teleport(Vector3 pos, Action<AvatarMovingResult> callback = null);

        ///<summary>
        /// Teleport avatar at given position
        ///</summary>
        /// <param name="marker">Target position marker</param>
        /// <param name="callback">Callback when moving complete. Flag tells whether the destination has been reached</param>
        void Teleport(IAvatarPositionMarker marker, Action<AvatarMovingResult> callback = null);

        /// <summary>
        ///  Сommand to move the avatar in a given position
        /// </summary>
        /// <param name="destination">Position to move</param>
        /// <param name="callback">Callback when moving complete. Flag tells whether the destination has been reached</param>
        /// <param name="teleportIfCanNotReached"></param>
        void SetDestination(Vector3 destination, Action<AvatarMovingResult> callback = null, bool teleportIfCanNotReached = false);

        ///<summary>
        /// Сommand to move the avatar in a given position marker take a pose
        /// <param name="marker">Target position marker</param>
        /// <param name="callback">Callback when moving complete. Flag tells whether the destination has been reached</param>
        ///</summary>
        void SetDestination(IAvatarPositionMarker marker, Action<AvatarMovingResult> callback = null);

        /// <summary>
        ///  Сommand to look at target position
        /// </summary>
        /// <param name="pos">Position to look at</param>
        /// <param name="callback">Look at complete Callback</param>
        void LookAt(Vector3 pos, Action callback = null);

        ///<summary>
        /// Сommand to look at active room camera position
        ///</summary>
        void LookAtActiveCamera();

        /// <summary>
        ///  Сommand to wait with idle animations for a given amount of time
        /// </summary>
        /// <param name="duration">Duration of the idle</param>
        /// <param name="callback">Idle complete callback</param>
        void SetIdle(float duration = 1.0f, Action callback = null);

        ///<summary>
        /// Set avatar's animator triger
        ///</summary>
        void SetAnimation(string triggerName, float duration = 3.0f);

        ///<summary>
        ///
        ///</summary>
        void SetupUI(IInRoomAvatarUI ui);

        ///<summary>
        ///
        ///</summary>
        void SetUIDirty();

        ///<summary>
        ///
        ///</summary>
        void ReleaseAvatarUIElementUser(IAvatarUIElementUser user);

        ///<summary>
        ///
        ///</summary>
        IAvatarUIElementUser GetAvatarUIElementUser(IUserTemplateSimple userTemplate);

        ///<summary>
        /// Prohibits the use of nav mesh.
        ///</summary>
        void DisableNavMeshUsage();

        ///<summary>
        /// Specifies which NavMesh areas are passable.
        ///</summary>
        void SetAreaMask(int areaMask);
    }
}
