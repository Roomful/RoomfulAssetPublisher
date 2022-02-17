using System;
using net.roomful.api.colorization;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// Roomful user template extensions.
    /// </summary>
    public static class UserTemplateExtensions
    {
        private const string FPS_MODE_TOKEN = "fps-mode";
        private const string USER_COLORIZATION_TOKEN = "user-colorization-override4";

        /// <summary>
        /// Returns custom user params.
        /// </summary>
        /// <param name="user">Target user.</param>
        /// <param name="callback">Callback when params are ready</param>
        public static void GetParams(this IUserTemplateSimple user, Action<IUserCustomParams> callback) {
            Roomful.UsersService.GetUserParams(user.Id, callback);
        }

        /// <summary>
        /// Returns user avatar image.
        /// </summary>
        /// <param name="template">Target user.</param>
        /// <param name="callback">Callback is fired when operation is complete.</param>
        public static void GetAvatar(this IUserTemplateSimple template, Action<Texture2D> callback) {
            Roomful.UsersService.GetUserAvatar(template, callback);
        }

        /// <summary>
        /// Returns games mode state
        /// </summary>
        public static void GetGamerModeState(this IUserTemplate user, IUsersService usersService, Action<bool> callback, bool defaultValue = false) {
            usersService.GetUserParams(user.Id, userParams => {
                var fpsModeState = defaultValue;
                if (userParams.Contains(FPS_MODE_TOKEN)) {
                    fpsModeState = userParams.Get<bool>(FPS_MODE_TOKEN);
                }
                callback(fpsModeState);
            });
        }

        /// <summary>
        /// Set's gamer mode state
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gamerModeState"></param>
        public static void SetGamerModeState(this IUserTemplate user, bool gamerModeState) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }
            Roomful.UsersService.SetCurrentUserParam(FPS_MODE_TOKEN, gamerModeState);
        }

        public static void GetColorizationSchemeType(this IUserTemplate user, IUsersService usersService, Action<ColorizationSchemeContext> callback) {
            usersService.GetUserParams(user.Id, userParams => {
                if (userParams.Contains(USER_COLORIZATION_TOKEN)) {
                    var doubleValue = userParams.Get<double>(USER_COLORIZATION_TOKEN);
                    callback.Invoke(new ColorizationSchemeContext() {
                        UseNetworkColorization = false,
                        ColorizationSchemeType = (ColorizationSchemeType)Convert.ToInt32(doubleValue)
                    });
                }
                else {
                    callback(new ColorizationSchemeContext() {
                        UseNetworkColorization = true
                    });
                }
            });
        }

        public static void SetColorizationSchemeType(this IUserTemplate user, ColorizationSchemeContext ctx) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }

            if (ctx.UseNetworkColorization) {
                Roomful.UsersService.RemoveCurrentUserParam(USER_COLORIZATION_TOKEN);
            }
            else {
                Roomful.UsersService.SetCurrentUserParam(USER_COLORIZATION_TOKEN, Convert.ToDouble((int)ctx.ColorizationSchemeType));
            }
        }

        public static void SetPrivacyMode(this IUserTemplate user, UserPrivacyMode mode) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }
            user.SetPrivacyMode(mode);
        }

        public static bool IsCurrentUser(this IUserTemplate user) {
            return Roomful.UsersService.CurrentUser.Id == user.Id;
        }
    }
}
