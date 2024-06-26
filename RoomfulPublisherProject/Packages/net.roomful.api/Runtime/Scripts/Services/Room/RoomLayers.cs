// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {

    public enum RoomLayers {
        Default = 0,
        IgnoreRaycast = 2,
        Wall = 8,
        Floor = 9,
        Ceiling = 10,
        Prop = 11,
        Room = 12,
        UI3D = 14,
        RoomBackLimitCollider = 17,
        DisabledArea = 18,
        Stand = 20,
        MirrorWall = 21,
        IgnorePostEffects = 23,
        
        /// <summary>
        /// Layer for Room Props that will be ignored by the real-time lights. E.g. Props that participated in Light Baking
        /// </summary>
        PropUnlit = 24,
        RoomSideWall = 25,
        TemporaryUsage = 26,

        /// <summary>
        /// Layer for AR mode
        /// </summary>
        AR = 30
    }
}
