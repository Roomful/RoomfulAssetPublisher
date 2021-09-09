using System;
using System.Collections.Generic;
using net.roomful.api.props;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api.avatars
{
    /// <summary>
    ///  An Accesses point to the in room 3D avatar's service.
    /// </summary>
    public interface IInRoomAvatarService
    {
        ///  <summary>
        ///  Returns the user's avatar, or random avatar, if the user has not setup avatar
        ///  </summary>
        /// <param name="user">The user who needs an avatar</param>
        ///  <param name="priority">Avatar loading priority</param>
        ///  <param name="callback">Callback when the avatar is loaded and ready to use</param>
        void CreateUserAvatar(IUserTemplateSimple user, AvatarRetrievalPriority priority, Action<IInRoomAvatar> callback);

        /// <summary>
        /// Create avatar based on <see cref="Avatar3DInfo"/> model.
        /// </summary>
        /// <param name="avatar3DInfo">Avatar data model.</param>
        /// <param name="priority">Avatar loading priority</param>
        /// <param name="callback">Callback when the avatar is loaded and ready to use</param>
        void CreateAvatar(Avatar3DInfo avatar3DInfo, AvatarRetrievalPriority priority, Action<IInRoomAvatar> callback);

        /// <summary>
        /// Creates default avatar fro current network.
        /// Can be used as fallback option if you can't create avatar you need.
        /// </summary>
        /// <param name="priority">Avatar loading priority</param>
        /// <param name="callback">Callback when the avatar is loaded and ready to use</param>
        void CreateDefaultAvatar(AvatarRetrievalPriority priority, Action<IInRoomAvatar> callback);

        /// <summary>
        /// Creates random avatar fro current network.
        /// Can be used as fallback option if you can't create avatar you need.
        /// </summary>
        /// <param name="priority">Avatar loading priority</param>
        /// <param name="callback">Callback when the avatar is loaded and ready to use</param>
        void CreateRandomAvatar(AvatarRetrievalPriority priority, Action<IInRoomAvatar> callback);

        /// <summary>
        /// Returns network related avatar assets.
        /// </summary>
        /// <param name="callback">Operation callback.</param>
        void GetNetworkAvatarsAssets(Action<Dictionary<string, IPropAssetTemplate>> callback);

        /// <summary>
        /// Use to find out if user has an avatar.
        /// </summary>
        /// <param name="callback">Callback returns `true` if customization exists and `false` otherwise.</param>
        void IsUserAvatarSet(Action<bool> callback);

        /// <summary>
        /// If you next action will have user avatar involved, it make seine to call this method
        /// to suggest user to build an avatar if he does not have one.
        ///
        /// Please note that user can delcine the request.
        /// </summary>
        /// <param name="callback">Operation callback.</param>
        void SuggestToSetAvatarIfCurrentUserDoesNotHaveIt(Action callback = null);

        /// <summary>
        /// Destroys avatar.
        /// </summary>
        /// <param name="avatar">Avatar to destroy.</param>
        void ReleaseAvatar(IInRoomAvatar avatar);

        /// <summary>
        /// This prop is assumed to be an avatar. If so, returns the avatar. If not, it returns null
        /// </summary>
        /// <param name="prop">Alleged avatar prop.</param>
        IInRoomAvatar GetAvatar(IProp prop);
    }
}
