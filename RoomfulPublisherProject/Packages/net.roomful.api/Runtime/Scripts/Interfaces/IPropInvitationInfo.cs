
// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    
    public interface IPropInvitationInfo {
        
        IPermissionsTemplate PermissionsTemplate { get; set; }
        string InvitedById { get; set; }
        IUserTemplate InvitedUser { get; set; }
    }
}
