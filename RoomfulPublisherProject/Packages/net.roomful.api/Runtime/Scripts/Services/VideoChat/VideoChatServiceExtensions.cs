using System.Collections.Generic;
using System.Linq;
using net.roomful.api;

namespace RF.Social
{
    public static class VideoChatServiceExtensions
    {
        public static IChatParticipantModel GetParticipantModel(this IVideoChatService @this, string userId) {
            return @this.ActiveChat.Status.Participants.FirstOrDefault(participant => participant.User.Id == userId);
        }
        
        public static IEnumerable<IUserTemplateSimple> GetActiveChatParticipants(this IVideoChatService @this) {
            return @this.ActiveChat.Status.Participants.Select(participant => participant.User).ToList();
        }

        public static bool TryGetVideoChatPresenter(this IVideoChatService @this, out IChatParticipantModel presenter) {
            presenter = null;

            if (@this.IsChatActive) {
                presenter = @this.ActiveChat.Status.Participants.FirstOrDefault(p => p.Permissions.IsPresenter);
                return presenter != null;
            }

            return false;
        }
        
        public static IChatParticipantModel GetVideoChatCoPresenter(this IVideoChatService @this) {
            if (@this.IsChatActive) {
                return @this.ActiveChat.Status.Participants.FirstOrDefault(p => p.Permissions.IsCoPresenter);
            }

            return null;
        }
        
        public static bool IsCurrentUserPresenterInVideoChat(this IVideoChatService @this) {
            if (@this.IsChatActive) {
                if (@this.GetModelForCurrentUser().Permissions.IsPresenter) {
                    return true;
                }
            }

            return false;
        }

        public static IChatParticipantModel GetModelForCurrentUser(this IVideoChatService @this) {
            return @this.ActiveChat.Status.Participants.FirstOrDefault(p => p.User.Id == Roomful.UsersService.CurrentUser.Id);
        }
    }
}