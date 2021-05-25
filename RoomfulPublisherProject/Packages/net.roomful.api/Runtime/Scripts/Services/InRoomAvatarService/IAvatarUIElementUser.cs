using net.roomful.api.avatars.emotions;
using UnityEngine.Events;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    public interface IAvatarUIElementUser : IAvatarUIElement
    {
        IUserTemplateSimple User { get; }
        bool IsOnline { set; }
        bool IsTeamMember { set; }
        UnityAction OnClick { set; }
        void PlayAnimation(AvatarEmotions emotion);
    }
}
