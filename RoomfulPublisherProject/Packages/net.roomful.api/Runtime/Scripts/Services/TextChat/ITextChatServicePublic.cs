using net.roomful.api.props;

namespace net.roomful.api
{
    public interface IPublicTextChatService
    {
        void OpenRoomTextChat();
        void OpenPropTextChatChannel(IProp prop);
        void OpenTeamMemberAvatarDirectChat(IUserTemplateSimple user);
    }
}
