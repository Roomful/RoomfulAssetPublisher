#if UNITY_2018_3_OR_NEWER

using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Network.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace RF.AssetWizzard.Editor
{
    public class AccountTab : BaseWizardTab, IWizzardTab
    {
        public override string Name {
            get {
                return "Account";
            }
        }

        private TextField m_mailInput;
        private TextField m_passwordInput;
        private VisualContainer m_loginFormContainer;
        private VisualContainer m_logoutFormContainer;
        
        public AccountTab () {
            m_loginFormContainer = CreateLoginForm();
            m_logoutFormContainer = CreateLogoutForm();
            SetupForms();
        }

        private VisualContainer CreateLogoutForm() {
            var result = new VisualContainer();
            var promptLabel = new Label();
            promptLabel.text = "Username";
            result.Add(promptLabel);
            var mailLabel = new Label(AssetBundlesSettings.Instance.SessionId);
            result.Add(mailLabel);
            var logoutButton = new Button(LogoutButtonClickHandler);
            logoutButton.text = "Logout";
            result.Add(logoutButton);
            return result;
        }

        private VisualContainer CreateLoginForm() {
            var result = new VisualContainer();
            var promptLabel = new Label();
            promptLabel.text = "Use your Roomful account email and password to sign in.";
            promptLabel.style.fontSize = 18;
            result.Add(promptLabel);
            var mailLabel = new Label("Login");
            result.Add(mailLabel);
            m_mailInput = new TextField();
            result.Add(m_mailInput);
            var passwordLabel = new Label("Password");
            result.Add(passwordLabel);
            m_passwordInput = new TextField();
            m_passwordInput.isPasswordField = true;
            m_passwordInput.maskChar = '*';
            result.Add(m_passwordInput);
            var loginButton = new Button(LoginButtonClickHandler);
            loginButton.text = "Sign-in";
            result.Add(loginButton);
            return result;
        }

        private void SetupForms() {
            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                Debug.Log("Is Logged In");
                Add(m_logoutFormContainer);
                m_loginFormContainer.RemoveFromHierarchy();
            }
            else {
                Debug.Log("Is not Logged In");
                m_passwordInput.SetValueWithoutNotify(string.Empty);
                m_mailInput.SetValueWithoutNotify(string.Empty);
                Add(m_loginFormContainer);
                m_logoutFormContainer.RemoveFromHierarchy();
            }
        }

        private void LoginButtonClickHandler() {
            Debug.Log("Clicked " + m_passwordInput.value + " " + m_mailInput.value);
            var mail = m_mailInput.value;
            var password = m_passwordInput.value;
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(password)) {
                Debug.Log("Fill all inputs ");
            }
            else {
                new SignInCommand(mail, password).Execute(SignInCompeleteHandler);
            }
        }

        private void SignInCompeleteHandler(BaseCommandResult obj) {
            SetupForms();
        }

        private void LogoutButtonClickHandler() {
            new LogoutCommand().Execute(LogoutHandler);
        }

        private void LogoutHandler(BaseCommandResult obj) {
            SetupForms();
        }
    }
}


#endif