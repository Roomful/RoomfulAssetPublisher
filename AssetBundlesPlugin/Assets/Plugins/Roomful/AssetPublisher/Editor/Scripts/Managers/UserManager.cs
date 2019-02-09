using System;
using RF.AssetWizzard.Models;

namespace RF.AssetWizzard.Managers {

    public static class UserManager {

        public static event Action<UserTemplate> OnUserTemplateUpdate = delegate{};
        
        private static UserTemplate s_currentUser;

        public static void SetCurrentUser(UserTemplate user) {
            s_currentUser = user;
            OnUserTemplateUpdate(s_currentUser);
        }
    }
}