using System;
using System.Collections.Generic;
using net.roomful.api.props;

namespace net.roomful.api.textChat
{
    public interface IPublicTextChatService
    {
        void OpenRoomTextChat();
        void HideFullChat();
        void OpenPropTextChatChannel(IProp prop);
        void OpenTeamMemberAvatarDirectChat(IUserTemplateSimple user);
        void OpenDirectChatWithUser(IUserTemplateSimple user);
        void LazyLoadDiscoverUsers(Action<List<IUserTemplate>, bool> callback);
        void SetSearchQuery(string query);
        void ResetDiscover();
    }
}
