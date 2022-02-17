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
        RoomSideWall = 25,
        ClickableAnimationMarker = 29,
    }
}
