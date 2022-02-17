namespace net.roomful.api
{
    public interface IUserFriendTemplate : IUserTemplateSimple
    {
        void SetStatus(FriendState state);
        FriendState FriendState  { get; }
        
        string PhoneNumber { get; }
        string Email  { get; }
        
        int PublicRoomCount { get; }
        int ContactCount  { get; }

        void SetPhoneNumber(string phone);
    }
}