// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.authentication
{
    /// <summary>
    /// Authentication Result model.
    /// </summary>
    public class AuthenticationResult
    {
        /// <summary>
        /// Flag shows if user has authed as anonymous.
        /// </summary>
        public bool IsAnonymous { get; } = false;

        /// <summary>
        /// Login source used by a user.
        /// </summary>
        public LoginSource Source { get; }

        /// <summary>
        /// Session id.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// Create new auth result.
        /// </summary>
        /// <param name="sessionId">Session id.</param>
        /// <param name="source">Login source.</param>
        /// <param name="isAnon">Set `true` if user chose to auth as anonymous.</param>
        public AuthenticationResult(string sessionId, LoginSource source, bool isAnon) {
            SessionId = sessionId;
            Source = source;
            IsAnonymous = isAnon;
        }
    }
}
