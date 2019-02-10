#if UNITY_2018_3_OR_NEWER

using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Managers;
using RF.AssetWizzard.Models;
using RF.AssetWizzard.Results;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Button = UnityEngine.Experimental.UIElements.Button;

namespace RF.AssetWizzard.Editor
{
    public class AccountTab : BaseWizardTab, IWizzardTab
    {
        public override string Name => "Account";
        
        private const string USER_LABEL_AVATAR = "UserAvatar";
        private const string USER_LABEL_NAME = "UserFullName";
        private const string USER_LABEL_EMAIL = "Email";
        
        private const string LOGOUT_BUTTON_TEXT = "Logout";
        private const int LOGOUT_BUTTON_MAX_WIDTH = 200;
        private const int LOGOUT_BUTTON_MIN_WIDTH = 200;
        
        private const string LOGIN_DESCRIPTION_LABEL_TEXT = "Use your Roomful account email and password to sign in.";
        private const string EMAIL_LABEL_TEXT = "E-mail:";
        private const string LOGIN_PASSWORD_LABEL_TEXT = "Password:";
        private const string LOGIN_BUTTON_TEXT = "Sign-in";
        
        private TextField m_mailInput;
        private TextField m_passwordInput;
        
        private VisualContainer m_loginFormContainer;
        private VisualContainer m_logoutFormContainer;
        private VisualContainer m_userInfoContainer;
        private VisualContainer m_sessionIdContainer;

        private UserTemplate m_user;
        
        private const float AVATAR_MAX_WIDTH = 64f;
        private const float AVATAR_MAX_HEIGHT = 64f;
        
        public AccountTab () {
            m_loginFormContainer = CreateLoginForm();
            m_logoutFormContainer = CreateLogoutForm();
            SetupForms();
            
            UserManager.OnUserTemplateUpdate.AddListener(user => {
                m_user = user;
                var userFullNameLabel = m_userInfoContainer.Q<Label>(USER_LABEL_NAME);
                userFullNameLabel.text = m_user.FullName();
                
                var userEmailLabel = m_userInfoContainer.Q<Label>(USER_LABEL_EMAIL);
                userEmailLabel.text = m_user.Email;
                
                var userAvatarLabel = m_userInfoContainer.Q<Label>(USER_LABEL_AVATAR);
                m_user.GetAvatar(tex => {
                    var width = (float)tex.width;
                    var height = (float)tex.height;
                    TextureResizer.ResizeTextureMaintainAspectRatio(AVATAR_MAX_WIDTH, AVATAR_MAX_HEIGHT, ref width, ref height);
                    
                    userAvatarLabel.style.width = width;
                    userAvatarLabel.style.height = height;
                    userAvatarLabel.style.backgroundImage = tex;
                });
            });
        }

        private VisualContainer CreateLoginForm() {
            m_mailInput = new TextField();
            m_mailInput.style.minWidth = 300;
            m_mailInput.style.marginLeft = 150;
            
            m_passwordInput = new TextField {
                isPasswordField = true,
                maskChar = '*'
            };
            m_passwordInput.style.minWidth = 300;
            m_passwordInput.style.marginLeft = 132;

            var emailContainer = new VisualContainer {
                new Label {
                    text = EMAIL_LABEL_TEXT
                },
                m_mailInput
            };
            emailContainer.style.flexDirection = FlexDirection.Row;

            var passwordContainer = new VisualContainer {
                new Label {
                    text = LOGIN_PASSWORD_LABEL_TEXT
                },
                m_passwordInput
            };
            passwordContainer.style.flexDirection = FlexDirection.Row;
            
            var loginContainer = new VisualContainer {
                new Label {
                    text = LOGIN_DESCRIPTION_LABEL_TEXT
                },
                emailContainer,
                passwordContainer,
                new Button(LoginButtonClickHandler) {
                    text = LOGIN_BUTTON_TEXT,
                    style = {
                        // flexGrow = 0.5f
                        maxWidth = LOGOUT_BUTTON_MAX_WIDTH,
                        minWidth = LOGOUT_BUTTON_MIN_WIDTH
                    }
                }
            };

            return loginContainer;
        }

        private VisualContainer CreateLogoutForm() {
            var userInfo = new VisualContainer {
                new Label {
                    name = USER_LABEL_NAME
                },
                new Label {
                    name = USER_LABEL_EMAIL
                }
            };
            userInfo.style.flexDirection = FlexDirection.Column;
            
            m_userInfoContainer = new VisualContainer {
                new Label {
                    name = USER_LABEL_AVATAR
                },
                userInfo
            };
            m_userInfoContainer.style.flexDirection = FlexDirection.Row;
           
            var logoutContainer = new VisualContainer {
                m_userInfoContainer,
                new Button(LogoutButtonClickHandler) {
                    text = LOGOUT_BUTTON_TEXT,
                    style = {
                       // flexGrow = 0.5f
                        maxWidth = LOGOUT_BUTTON_MAX_WIDTH,
                        minWidth = LOGOUT_BUTTON_MIN_WIDTH
                    }
                }
            };
            
            return logoutContainer;
        }

        private void SetupForms() {
            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                UserManager.Authenticate();
                m_loginFormContainer.RemoveFromHierarchy();
                Add(m_logoutFormContainer);
            }
            else {
                m_passwordInput.SetValueWithoutNotify(string.Empty);
                m_mailInput.SetValueWithoutNotify(string.Empty);
                m_logoutFormContainer.RemoveFromHierarchy();
                Add(m_loginFormContainer);
            }
        }
        
        private void LoginButtonClickHandler() {
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