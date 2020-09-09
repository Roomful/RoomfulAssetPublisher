using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StansAssets.Foundation;

namespace net.roomful.assets.Editor
{
    internal class AccountPanel : Panel
    {
        private string m_mail = string.Empty;
        private string m_password = string.Empty;

        public AccountPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() {
            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                GUILayout.Label("Use your Roomful account email and password to sign in.");

                AuthWindow();
                return;
            }

            GUILayout.Label("Roomful asset wizzard. Logged in as: " + AssetBundlesSettings.Instance.SessionId);

            if (GUILayout.Button("Log Out")) {
                m_mail = string.Empty;
                m_password = string.Empty;

                AssetBundlesSettings.Instance.SetSessionId(string.Empty);
                BundleUtility.ClearLocalCache();
            }
        }

        private void AuthWindow() {
            GUILayout.BeginVertical();

            m_mail = EditorGUILayout.TextField("E-mail: ", m_mail);
            m_password = EditorGUILayout.PasswordField("Password: ", m_password);

            if (GUILayout.Button("Log In")) {
                if (string.IsNullOrEmpty(m_mail) || string.IsNullOrEmpty(m_password)) {
                    Debug.Log("Fill all inputs ");
                }
                else {
                    var signInRequest = new Network.Request.Signin(m_mail, m_password);

                    signInRequest.PackageCallbackText = signInCallback => {
                        ParseSessionToken(signInCallback);
                    };

                    signInRequest.Send();
                }
            }

            GUILayout.EndVertical();
        }

        private void ParseSessionToken(string resp) {
            var originalJson = Json.Deserialize(resp) as Dictionary<string, object>;
            AssetBundlesSettings.Instance.SetSessionId(originalJson["session_token"].ToString());
        }
    }
}