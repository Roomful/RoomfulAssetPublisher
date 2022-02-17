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
        /// Event is fired on user reconnect
        /// </summary>
        event Action OnReconnect;

        /// <summary>
        /// Use to set own auth controller.
        /// </summary>
        /// <param name="control">Auth controller.</param>
        void SetAuthenticationController(IAuthenticationController control);

        /// <summary>
        /// Returns user activity counters after login is performed.
        /// </summary>
        /// <returns></returns>
        ILoginUserActivityCounters GetLoginActivityCounters();

        /// <summary>
        /// Use to offer anonymous user login pop up
        /// </summary>
        void OfferStartSession();

        /// <summary>
        /// Will end session for the current user.
        /// </summary>
        void EndSession();

        /// <summary>
        /// Use to set session id for SSO.
        /// </summary>
        /// <param name="sessionId"> Session id that we get from server after successful authentication.</param>
        void SingleSignOnCallback(string sessionId);

        /// <summary>
        /// For testing purposes only.
        /// </summary>
        void EmulateSingIn(string mail, string password);


        /// <summary>
        /// Should be for internal use only. But public so far while we in transition to the asm def.
        /// </summary>
        void OfferChangePassword();
    }
}
