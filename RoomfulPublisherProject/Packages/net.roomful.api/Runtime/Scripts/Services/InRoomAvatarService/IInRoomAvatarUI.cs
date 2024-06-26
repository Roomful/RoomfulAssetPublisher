using System.Collections.Generic;
using net.roomful.api.avatars.emotions;
using UnityEngine;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    public interface IInRoomAvatarUI
    {
        Transform Transform { get; }
        void HideUI();
        void PlayAvatarEmotionAnimation(AvatarEmotions emotion);
        IReadOnlyCollection<IAvatarUIElement> GetItems();
    }
}
