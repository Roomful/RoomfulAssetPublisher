using UnityEngine;
using UnityEngine.UI;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    public interface IInRoomAvatarUIButton
    {
        RectTransform RectTransform { get; }
        Button Button { get; }
    }
}
