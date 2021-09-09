using System;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.authentication
{
    public interface IAuthenticationController
    {
        event Action<AuthenticationResult> OnAuthResult;
        void Show(LoginWindowPage page, Action onShow = null);
        void Hide(Action onHide = null);

        bool IsLoginPageActive { get; }
    }

    public enum LoginWindowPage
    {
        Main,
        SignIn,
        SignUp,
        ChangePassword,
        EnterPassword
    }
}
