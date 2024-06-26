using System;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.authentication
{
    /// <summary>
    /// The Authentication controller.
    /// </summary>
    public interface IAuthenticationController
    {
        /// <summary>
        /// Event when auth is completed with the result.
        /// </summary>
        event Action<AuthenticationResult> OnAuthResult;

        /// <summary>
        /// Show auth view on a specific page.
        /// </summary>
        /// <param name="page">Auth view page.</param>
        /// <param name="onShow">Event is fired when page is fully shown.</param>
        void Show(LoginWindowPage page, Action onShow = null);

        /// <summary>
        /// Hides auth view.
        /// </summary>
        /// <param name="onHide">Event is fired when page hide flow is completed.</param>
        void Hide(Action onHide = null);

        /// <summary>
        /// Returns `true` if login page is active and `false` otherwise
        /// </summary>
        bool IsLoginPageActive { get; }

        void EmulateSingIn(string mail, string password);
    }

    /// <summary>
    /// Available login pages.
    /// </summary>
    public enum LoginWindowPage
    {
        /// <summary>
        /// Main welcome page.
        /// </summary>
        Main,

        /// <summary>
        /// Sing In page.
        /// </summary>
        SignIn,

        /// <summary>
        /// Sing Up page.
        /// </summary>
        SignUp,

        /// <summary>
        /// Change Password page.
        /// </summary>
        ChangePassword,

        /// <summary>
        /// Enter Password page.
        /// </summary>
        EnterPassword
    }
}
