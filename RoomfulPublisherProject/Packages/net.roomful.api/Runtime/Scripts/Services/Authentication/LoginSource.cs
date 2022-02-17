// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.authentication
{
    /// <summary>
    /// Available login sources for user.
    /// </summary>
    public enum LoginSource
    {
        /// <summary>
        /// Logged via Facebook.
        /// </summary>
        Facebook,

        /// <summary>
        /// Login by e-mail.
        /// </summary>
        Email,

        /// <summary>
        /// Login by locally stored session id.
        /// </summary>
        SessionId,

        /// <summary>
        /// Logged in as Anonymous.
        /// </summary>
        Anonymous
    }
}
