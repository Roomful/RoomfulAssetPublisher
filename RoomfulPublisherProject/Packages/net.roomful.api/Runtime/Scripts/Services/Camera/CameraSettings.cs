using UnityEngine;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.cameras
{
    /// <summary>
    /// Camera position settings.
    /// </summary>
    public class CameraSettings
    {
        /// <summary>
        /// Target camera rotation.
        /// Where it wants to be, but it bot nestlers there yet.
        /// Or in some cases it may never go that, like if it will go out of bounds.
        /// </summary>
        public Vector3 TargetRotation;

        /// <summary>
        /// Target camera position.
        /// Where it wants to be, but it bot nestlers there yet.
        /// Or in some cases it may never go that, like if it will go out of bounds.
        /// </summary>
        public Vector3 TargetPosition;
    }
}
