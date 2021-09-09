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

        /// <summary>
        /// Event is fired on user is logged in.
        /// </summary>
        event Action OnLogIn;

        /// <summary>
        /// Use to set own auth controller.
        /// </summary>
        /// <param name="control">Auth controller.</param>
        void SetAuthenticationController(IAuthenticationController control);

        ILoginUserActivityCounters GetLoginActivityCounters();
        
        /// <summary>
        /// Use to offer anonymous user login pop up
        /// </summary>
        void OfferStartSession();
    }
}
