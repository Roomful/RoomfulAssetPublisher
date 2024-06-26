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
        const string k_FPSModeToken = "fps-mode";
        const string k_UserColorizationToken = "user-colorization-override4";
        const string k_CameraModeToken = "CameraMode";

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
            Roomful.UsersService.GetUserAvatar(template.Id, callback);
        }

        /// <summary>
        /// True means that WASD for movements and Arrows for rotation
        /// </summary>
        public static void GetKeyboardLayout(this IUserTemplate user, Action<bool> callback, bool defaultValue = false) {
            Roomful.UsersService.GetUserParams(user.Id, userParams => {
                var fpsModeState = defaultValue;
                if (userParams.Contains(k_FPSModeToken)) {
                    fpsModeState = userParams.Get<bool>(k_FPSModeToken);
                }
                callback(fpsModeState);
            });
        }

        /// <summary>
        /// Set's gamer keyboard layout
        /// </summary>
        /// <param name="user">User template</param>
        /// <param name="gamerKeyboardLayout">Gamer keyboard layout</param>
        public static void SetKeyboardLayout(this IUserTemplate user, bool gamerKeyboardLayout) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }
            Roomful.UsersService.SetCurrentUserParam(k_FPSModeToken, gamerKeyboardLayout);
        }

        public static void GetColorizationSchemeType(this IUserTemplate user, IUsersService usersService, Action<ColorizationSchemeContext> callback) {
            usersService.GetUserParams(user.Id, userParams => {
                if (userParams.Contains(k_UserColorizationToken)) {
                    var doubleValue = userParams.Get<double>(k_UserColorizationToken);
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
                Roomful.UsersService.RemoveCurrentUserParam(k_UserColorizationToken);
            }
            else {
                Roomful.UsersService.SetCurrentUserParam(k_UserColorizationToken, Convert.ToDouble((int)ctx.ColorizationSchemeType));
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

        public static void GetCameraMode(this IUserTemplate user, Action<CameraMode> callback, CameraMode defaultValue = CameraMode.Classic) {
            Roomful.UsersService.GetUserParams(user.Id, userParams => {
                CameraMode avatarMode = defaultValue;
                if (userParams.Contains(k_CameraModeToken)) {
                    if (Enum.TryParse<CameraMode>(userParams.Get<string>(k_CameraModeToken), out var result)) {
                        avatarMode = result;
                    }
                }
                callback(avatarMode);
            });
        }
        
        public static void SetCameraMode(this IUserTemplate user, CameraMode avatarMode) {
            if (Roomful.UsersService.CurrentUser.Id != user.Id) {
                Debug.LogWarning("[ACCESS DENIED] You CAN'T change UserParams of other Users");
                return;
            }
            Roomful.UsersService.SetCurrentUserParam(k_CameraModeToken, avatarMode.ToString());
        }
    }

    public enum CameraMode
    {
        Classic = 0,
        Follow = 1
    }
}
