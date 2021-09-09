using System;
using net.roomful.api;
using UnityEngine;

namespace net.roomful
{
    public static class UserTemplateExtensions
    {
        private const string FPS_MODE_TOKEN = "fps-mode";

        public static void GetParams(this IUserTemplateSimple user, Action<IUserCustomParams> callback) {
            Roomful.UsersService.GetUserParams(user.Id, callback);
        }

        public static void GetAvatar(this IUserTemplateSimple template, Action<Texture2D> callback) {
            Roomful.UsersService.GetUserAvatar(template, callback);
        }

        public static void GetGamerModeState(this IUserTemplate user, Action<bool> callback, bool defaultValue = false) {
            Roomful.UsersService.GetUserParams(user.Id, userParams => {
                var fpsModeState = defaultValue;
                if (userParams.Contains(FPS_MODE_TOKEN)) {
                    fpsModeState = userParams.Get<bool>(FPS_MODE_TOKEN);
                }
                callback(fpsModeState);
            });
        }

        public static void SetGamerModeState(this IUserTemplate user, bool gamerModeState) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }
            Roomful.UsersService.SetCurrentUserParam(FPS_MODE_TOKEN, gamerModeState);
        }
    }
}
