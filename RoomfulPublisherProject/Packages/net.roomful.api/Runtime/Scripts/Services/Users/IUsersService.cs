using System;
using System.Collections.Generic;
using net.roomful.models;
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
        /// Event is fired when current user is link or unlink to
        /// some social account.
        /// </summary>
        event Action OnSocialAccountsUpdated;

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
        /// <param name="userId">User Id.</param>
        /// <param name="callback">Request callback.</param>
        void GetUserAvatar(string userId, Action<Texture2D> callback);

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
        void GetFullUserInfo(string userId, Action<GetUserInfoResult> callback);

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
        /// Decline friend request that was sent to target user.
        /// </summary>
        /// <param name="userId">Target user id.</param>
        void DeleteFriendRequest(string userId);

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
        void SelectUsers(bool addCurrentUser, IReadOnlyList<IUserTemplateSimple> preselectedUsers, Action<ContactsSelectionResult> callback);

        /// <summary>
        /// Update <see cref="Avatar3DInfo"/> for a given user id
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="avatar3DInfo">New info for 3D avatar</param>
        void SetUser3DAvatar(string userId, Avatar3DInfo avatar3DInfo);

        /// <summary>
        /// Returns Linked social account info.
        /// If account with the provided kind weren't liked, callback will return null.
        /// </summary>
        /// <param name="accountKind">
        /// Social account kind, so far we have:
        /// google/apple/facebook/epam/verus
        /// </param>
        /// <param name="callback">Operation result callback.</param>
        void GetSocialAccount(SocialAccountKind accountKind, Action<ISocialAccount> callback);

        /// <summary>
        /// Fetches list of the user linked social accounts.
        /// </summary>
        /// <param name="callback">Operation callback.</param>
        void GetSocialAccounts(Action<IReadOnlyCollection<ISocialAccount>> callback);
    }
    
    /// <summary>
    /// User info load result.
    /// </summary>
    public struct GetUserInfoResult
    {
        /// <summary>
        /// True if user info retrieve was as success, false otherwise.
        /// </summary>
        public bool IsSuccess => User!= null;


        /// <summary>
        /// True if user info retrieve was unsuccessful, false otherwise.
        /// </summary>
        public bool IsFailed => !IsSuccess;
        
        /// <summary>
        /// User template.
        /// </summary>
        public IUserTemplate User;
    }
}
