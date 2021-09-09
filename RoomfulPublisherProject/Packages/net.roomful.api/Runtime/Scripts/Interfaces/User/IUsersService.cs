using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    /// <summary>
    /// Provides users access point.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Event is fired when current user params are updated.
        /// </summary>
        event Action OnUserCustomParamsUpdated;

        /// <summary>
        /// Event is fired when current user model is updated.
        /// </summary>
        event Action OnCurrentUserUpdated;

        /// <summary>
        /// Event is fired when user info cache is updated.
        /// This also includes updated for the current user.
        /// </summary>
        event Action<IUserTemplateSimple> OnUserInfoUpdated;

        event Action<IUserTemplateSimple> OnUserCustomizationUpdated;

        /// <summary>
        /// Currently logged user.
        /// </summary>
        IUserTemplate CurrentUser { get; }

        /// <summary>
        /// Active users friends data.
        /// </summary>
        IPaginatedDataSource<IUserFriendTemplate> UserFriends { get; }

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

        /// <summary>
        /// Method to retrieve user template.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="callback">Request callback.</param>
        void GetUserInfo(string userId, Action<IUserTemplateSimple> callback);

        /// <summary>
        /// Method to retrieve all available user info.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        /// <param name="callback">Request callback.</param>
        void GetFullUserInfo(string userId, Action<IUserTemplate> callback);

        /// <summary>
        /// Set custom user param.
        /// </summary>
        /// <param name="key">Param Key.</param>
        /// <param name="value">Param Value.</param>
        void SetCurrentUserParam(string key, object value);

        /// <summary>
        /// Remove user custom param.
        /// </summary>
        /// <param name="key">Param Key.</param>
        void RemoveCurrentUserParam(string key);

        /// <summary>
        /// Accept user friend request.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        void AcceptFriend(string userId);

        /// <summary>
        /// Decline user friend request.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        void DeclineFriend(string userId);

        /// <summary>
        /// Delete user from friends.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        void DeleteFriend(string userId);

        /// <summary>
        /// Send friend request for a user
        /// </summary>
        /// <param name="userId">Target user id.</param>
        void SendFriendRequest(string userId);

        /// <summary>
        /// Method will send Roomful invite to the user with specified e-mail.
        /// </summary>
        /// <param name="usersEmail"></param>
        void InviteToRoomful(string usersEmail);
        IFriendsDataSource InstantiateFriendsDataSource();
        void SelectUsers(bool addCurrentUser, List<IUserFriendTemplate> preselectedUsers, Action<ContactsSelectionResult> callback);
    }
}
