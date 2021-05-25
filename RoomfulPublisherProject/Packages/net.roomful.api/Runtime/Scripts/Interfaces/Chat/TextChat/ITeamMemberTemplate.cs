namespace net.roomful.api {

    public interface ITeamMemberTemplate {
        
        IVideoChatTemplateSimple VideoChat { get; }
        IUserTemplateSimple User { get; }
    }
}