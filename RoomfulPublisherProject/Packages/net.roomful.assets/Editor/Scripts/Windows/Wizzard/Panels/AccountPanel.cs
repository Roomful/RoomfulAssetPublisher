using UnityEditor;
using UnityEngine;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.editor
{
    class AccountPanel : Panel
    {
        string m_Mail = string.Empty;
        string m_Password = string.Empty;

        protected override bool RequireAuth => false;

        bool m_RequestInProgress;
        Vector2 m_KeyScrollPos;
        
        public AccountPanel(EditorWindow window) : base(window)
        {
            RoomfulPublisher.Session.OnLogOut += OnLogOut;
        }

        public override void OnGUI() {
            if (m_RequestInProgress)
            {
                DrawPreloaderAt(new Rect(280, 12, 20, 20));
                GUI.enabled = false;
            }
            
            if (AssetBundlesSettings.Instance.IsLoggedOut) {
                AuthWindow();
                return;
            }
            
            if (RoomfulPublisher.Session.LoggedUser != null)
            {
                DrawUserInfo();
                DrawAvailableNetworks();
            }
            else
            {
                if (!m_RequestInProgress)
                {
                    LoadUserModel();
                }
            }
        }


        RoomfulNetwork m_SelectedNetwork;
        
        void DrawAvailableNetworks()
        {
            var networks = RoomfulPublisher.Session.Networks.Models;
            using (new IMGUIBeginHorizontal(WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(445)))
            {
                var s = new GUIStyle(EditorStyles.boldLabel);
                s.margin = new RectOffset(0, 0, 0, 0);
                s.padding = new RectOffset(2, 2, 2, 2);

                GUILayout.Label("Available Networks", s, GUILayout.Width(130));
            }

            const int assetsListWidth = 215;
            using (new IMGUIBeginHorizontal())
            {
                GUILayout.Space(1);
                using (new IMGUIBeginVertical(GUILayout.Width(assetsListWidth)))
                {
                    if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.ToString().Contains("Asset Publisher"))
                        m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(assetsListWidth), GUILayout.MaxHeight(EditorWindow.GetWindow<WizardWindow>(false, "Roomful - Asset Publisher").position.height), GUILayout.ExpandHeight(true));
                    else
                        m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(assetsListWidth), GUILayout.ExpandHeight(true));



                    GUI.Box(new Rect(0, 0, assetsListWidth - 5, networks.Count * 16.25f + 26), "", WizardWindow.Constants.settingsBox);
                    Texture assetImage = new Texture2D(0, 0);
                    foreach (var network in networks)
                    {
                        if (GUILayout.Toggle(m_SelectedNetwork == network, network.Name, WizardWindow.Constants.keysElement, GUILayout.Width(assetsListWidth - 5)))
                        {
                            m_SelectedNetwork = network;
                        }
                    }
                    GUILayout.EndScrollView();
                }

                GUILayout.Space(5);
                using (new IMGUIBeginVertical(GUILayout.ExpandWidth(true)))
                {
                    if (m_SelectedNetwork != null)
                    {
                        using (new IMGUIBeginHorizontal(GUILayout.ExpandWidth(true)))
                        {
                            GUILayout.Label("Selected Network", WizardWindow.Constants.settingsBoxTitle);

                            var openAsset = GUILayout.Button("Apply", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(40), GUILayout.Height(20));
                            if (openAsset)
                            {
                                // BundleService.Download(m_SelectedAsset);
                            }
                        }

                        EditorGUILayout.Space();

                        AssetInfoLabel("Id", m_SelectedNetwork.Id);
                        AssetInfoLabel("Name", m_SelectedNetwork.Name);

                        EditorGUILayout.Space();
                    }
                }

                GUILayout.Space(10f);
            }
        }
        
        protected void AssetInfoLabel(string title, object msg) {
            GUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(title)) {
                title += ": ";
            }

            EditorGUILayout.LabelField(title, EditorStyles.boldLabel, GUILayout.Height(16), GUILayout.Width(65));
            EditorGUILayout.SelectableLabel(msg.ToString(), EditorStyles.label, GUILayout.Height(16));
            GUILayout.EndHorizontal();
        }

        public void OnLogOut()
        {
            m_Mail = string.Empty;
            m_Password = string.Empty;
            m_RequestInProgress = false;
        }

        void DrawUserInfo()
        {
            var loggedUser = RoomfulPublisher.Session.LoggedUser;
            var loggedUserAvatar = RoomfulPublisher.Session.LoggedUserAvatar;
            using (new IMGUIBeginHorizontal())
            {
                using (new IMGUIBeginVertical(GUILayout.Width(370)))
                {
                    PrintUserField("Full Name", loggedUser.FullName);
                    PrintUserField("Tittle", loggedUser.CompanyTitle);
                    PrintUserField("Company", loggedUser.CompanyName);
                }

                using (new IMGUIBeginVertical(GUILayout.Width(70)))
                {
                    EditorGUILayout.ObjectField(loggedUserAvatar, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                    if (loggedUserAvatar == null)
                    {
                        DrawPreloaderAt(new Rect(590, 63, 20, 20));
                    }
                    
                    GUILayout.Space(5);
                    if (GUILayout.Button("Log Out")) {
                        RoomfulPublisher.Session.LogOut();
                    }
                }
            }
        }
        void AuthWindow() {
            
            GUILayout.Label("Use your Roomful account email and password to sign in.");
            GUILayout.Label("If you don't have an account yet, visit https://roomful.net.");
            GUILayout.Space(5);
            
            using (new IMGUIBeginVertical(GUILayout.Width(450)))
            {
                m_Mail = EditorGUILayout.TextField("E-mail: ", m_Mail);
                m_Password = EditorGUILayout.PasswordField("Password: ", m_Password);
                
                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Log In"))
                    {
                        if (string.IsNullOrEmpty(m_Mail) || string.IsNullOrEmpty(m_Password))
                        {
                            EditorUtility.DisplayDialog("Note", "Please type in login and password.", "Fine");
                        }
                        else
                        {
                            m_RequestInProgress = true;
                            RoomfulPublisher.Session.SignIn(m_Mail, m_Password, () =>
                            {
                                m_RequestInProgress = false;
                            });
                        }
                    }
                }
            }
        }

        void PrintUserField(string name, string value)
        {
            using (new IMGUIBeginHorizontal())
            {
                EditorGUILayout.LabelField($"{name}:", GUILayout.Width(100));
                using (new IMGUIEnable(false))
                {
                    EditorGUILayout.TextField(value, GUILayout.Width(240));
                }
            }
        }
        
        void LoadUserModel()
        {
            m_RequestInProgress = true;
            RoomfulPublisher.Session.LoadUserInfo(() =>
            {
                m_RequestInProgress = false;
            });
        }
    }
}