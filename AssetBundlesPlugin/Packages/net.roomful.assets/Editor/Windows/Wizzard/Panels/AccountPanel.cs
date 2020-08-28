using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;
using StansAssets.Foundation;

namespace RF.AssetWizzard.Editor
{
    public class AccountPanel : Panel
    {

        private string Mail = string.Empty;
        private string Password = string.Empty;


        public AccountPanel(EditorWindow window) : base(window) { }



        public override void OnGUI() {

            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                GUILayout.Label("Use your Roomful account email and password to sign in.");

                AuthWindow();
                return;
            }


            GUILayout.Label("Roomful asset wizzard. Logged in as: " + AssetBundlesSettings.Instance.SessionId);

            if (GUILayout.Button("Log Out")) {
                Mail = string.Empty;
                Password = string.Empty;

                AssetBundlesSettings.Instance.SetSessionId(string.Empty);
                BundleUtility.ClearLocalCache();
            }
        }


        private void AuthWindow() {
            GUILayout.BeginVertical();

            Mail = EditorGUILayout.TextField("E-mail: ", Mail);
            Password = EditorGUILayout.PasswordField("Password: ", Password);

            if (GUILayout.Button("Log In")) {
                if (string.IsNullOrEmpty(Mail) || string.IsNullOrEmpty(Password)) {
                    Debug.Log("Fill all inputs ");
                } else {
                    Network.Request.Signin signInRequest = new RF.AssetWizzard.Network.Request.Signin(Mail, Password);

                    signInRequest.PackageCallbackText = (signInCallback) => {
                        ParseSessionToken(signInCallback);
                    };

                    signInRequest.Send();
                }
            }

            GUILayout.EndVertical();
        }

        private void ParseSessionToken(string resp) {
            Dictionary<string, object> originalJson = Json.Deserialize(resp) as Dictionary<string, object>;
            AssetBundlesSettings.Instance.SetSessionId(originalJson["session_token"].ToString());
        }
    }
}