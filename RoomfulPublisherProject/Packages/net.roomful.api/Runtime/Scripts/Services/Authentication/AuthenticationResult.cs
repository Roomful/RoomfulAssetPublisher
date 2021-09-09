// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.authentication
{
    public class AuthenticationResult
    {
        public bool IsAnonymous { get; } = false;

        public LoginSource Source { get; }

        public string SessionId { get; }

        public AuthenticationResult(string sessionId, LoginSource source, bool isAnon) {
            SessionId = sessionId;
            Source = source;
            IsAnonymous = isAnon;
        }
    }
}
