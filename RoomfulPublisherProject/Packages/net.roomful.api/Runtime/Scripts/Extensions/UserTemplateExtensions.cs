using System;
using net.roomful.api;
using UnityEngine;

namespace net.roomful
{
    public static class UserTemplateExtensions
    {
        public static void GetParams(this IUserTemplateSimple user, Action<IUserCustomParams> callback) {
            Roomful.UsersService.GetUserParams(user.Id, callback);
        }

        public static void GetAvatar(this IUserTemplateSimple template, Action<Texture2D> callback) {
            Roomful.UsersService.GetUserAvatar(template, callback);
        }
    }
}
