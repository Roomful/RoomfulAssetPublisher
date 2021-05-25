using System;

namespace net.roomful.api
{
    public interface IUserTemplate : IUserFriendTemplate
    {
       
        string Birthday  { get; }
        string Hometown  { get; }
        string Education { get; }
        string Description { get; }
        string LinkUrl { get; }
        
        string SharedEmail { get; }
        string SharedPhone { get; }
        bool EmailVerified { get; }
        DateTime Created  { get; }
        string CreatedRFC3339  { get; }
        int CommonFriends { get; }
        UserFacebookLink FacebookLink  { get; }
        IEmployeeTemplate EmployeeTemplate { get; }
        UserRoles Roles { get; }
        string Serialize(bool includeBase64 = true);
        void SetLinkUrl(string newLinkUrl);
        void SetDescription(string description);
        void SetPublicRoomNewCounter(int count);
    }
}