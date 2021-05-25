using System;

namespace net.roomful.api.authentication
{
    /// <summary>
    /// Allows to monitor application authentication related events,
    /// as well as control user authentication status.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Event is fired on user logOut.
        /// </summary>
        event Action OnLogout;
    }
}
