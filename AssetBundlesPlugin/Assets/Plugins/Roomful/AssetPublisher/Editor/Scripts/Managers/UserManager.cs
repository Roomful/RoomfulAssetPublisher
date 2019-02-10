using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Models;
using SA.Foundation.Events;
using UnityEngine;

namespace RF.AssetWizzard.Managers {

    public static class UserManager {

        public static SA_Event<UserTemplate> OnUserTemplateUpdate = new SA_Event<UserTemplate>();
        
        private static UserTemplate s_currentUser;

        public static void Authenticate() {
            if (!IsUserExists) {
                new GetUserTemplateCommand().Execute(result => {
                    s_currentUser = result.User;
                    OnUserTemplateUpdate.Invoke(s_currentUser);
                });
            }
            else {
                OnUserTemplateUpdate.Invoke(s_currentUser);
            }
        }

        private static bool IsUserExists => s_currentUser != null;
    }
}