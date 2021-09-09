// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    /// <summary>
    ///  An accesses point to the user 1D avatar's prefab.
    /// </summary>
    public interface IPlayerAvatar1D
    {
        /// <summary>
        ///  Flag that the avatar can be clickable.
        /// </summary>
        bool Clickable { get; set; }

        /// <summary>
        /// Setup with user info.
        /// </summary>
        void SetUserInfo(IUserTemplateSimple info);
    }
}
