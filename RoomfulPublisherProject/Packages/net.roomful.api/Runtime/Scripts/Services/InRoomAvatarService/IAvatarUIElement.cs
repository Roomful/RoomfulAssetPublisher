using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    public interface IAvatarUIElement
    {
        RectTransform Transform { get; }
        bool IsActive { get; }
    }
}
