using System;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// Provides users access point.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Currently logged user.
        /// </summary>
        IUserTemplate CurrentUser { get; }

        /// <summary>
        /// Method to retrieve user additional params
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="callback">Request callback.</param>
        void GetUserParams(string userId, Action<IUserCustomParams> callback);

        /// <summary>
        /// Method to retrieve user avatar texture.
        /// </summary>
        /// <param name="template">User template.</param>
        /// <param name="callback">Request callback.</param>
        void GetUserAvatar(IUserTemplateSimple template, Action<Texture2D> callback);
    }
}
