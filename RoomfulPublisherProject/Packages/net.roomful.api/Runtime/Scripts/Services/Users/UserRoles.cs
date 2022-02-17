using System.Collections.Generic;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    public class UserRoles
    {
        public bool IsAdmin { get; } = false;
        public bool IsModerator { get; } = false;
        public bool IsPublisher { get; } = false;
        
        public UserRoles() { }

        public UserRoles(JSONData permissionsData) {
            if (permissionsData.HasValue("admin")) {
                IsAdmin = permissionsData.GetValue<bool>("admin");
            }

            if (permissionsData.HasValue("moderator")) {
                IsModerator = permissionsData.GetValue<bool>("moderator");
            }

            if (permissionsData.HasValue("publisher")) {
                IsPublisher = permissionsData.GetValue<bool>("publisher");
            }
        }

        public Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> data = new Dictionary<string, object>();

            data.Add("admin", IsAdmin);
            data.Add("moderator", IsModerator);
            data.Add("publisher", IsPublisher);

            return data;
        }
    }
}