using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    public class PublisherSession
    {
        bool m_IsAuthValidated;
        public event Action OnLogOut = delegate {  };
        public bool IsAuthed => !IsLoggedOut && m_IsAuthValidated;

        public NetworksContainer Networks { get; } = new NetworksContainer();

        public bool IsLoggedOut => AssetBundlesSettings.Instance.IsLoggedOut;
        public bool AuthCheckInProgress { get; private set; }
        public PublisherUser LoggedUser { get; private set; }
        public Texture2D LoggedUserAvatar { get; private set; }
        
        

        public void LogOut()
        {
            m_IsAuthValidated = false;
            AssetBundlesSettings.Instance.SetSessionId(string.Empty);
            BundleUtility.ClearLocalCache();
            Networks.Clear();
            
            LoggedUser = null;
            if (LoggedUserAvatar != null)
            {
                Object.DestroyImmediate(LoggedUserAvatar);
                LoggedUserAvatar = null;
            }
            
            OnLogOut.Invoke();
        }

        public void SignIn(string mail, string pass, Action callback)
        {
            var signInRequest = new Signin(mail, pass);
            signInRequest.Send(webPackageResult =>
            {
               
                if (webPackageResult.IsFailed)
                {
                    EditorUtility.DisplayDialog("Note", "Something went wrong, " +
                        "please make sure your login and password is correct.", "Ok");
                    callback.Invoke();
                    return;
                }
            
                var token = webPackageResult.DataAsDictionary["session_token"].ToString();
                AssetBundlesSettings.Instance.SetSessionId(token);
                
                CheckAuth(callback);
            });
        }

        public void LoadUserInfo(Action callback)
        {
            if (LoggedUser != null)
            {
                callback.Invoke();
                return;
            }
            
            var userInfo = new GetUserInfo();
            userInfo.Send(result =>
            {
                callback.Invoke();
                if (result.IsFailed)
                {
                    OnLogOut();
                    return;
                }
                
                var mainData = result.DataAsDictionary["data"]  as Dictionary<string, object>;
                var userData  = mainData["user"]  as Dictionary<string, object>;
                LoggedUser = new PublisherUser(userData);


                var url = $"/api/v0/resource/thumbnail/user/url/{LoggedUser.Id}";
                var getAvatarUrl = new CustomRequest(url, RequestMethods.GET);
                getAvatarUrl.Send(packageResult =>
                {
                    if (packageResult.IsFailed)
                    {
                        LoggedUserAvatar = new Texture2D(2, 2);
                        Debug.LogWarning("Failed to load user avatar");
                        return;
                    }
                        

                    var getAvatar = new CustomRequest(packageResult.DataAsText, RequestMethods.GET);
                    getAvatar.SetIsDataPack(true);
                    getAvatar.Send(getAvatarResult =>
                    {
                        var tex = new Texture2D(2, 2);
                        if (getAvatarResult.IsSucceeded)
                        {
                            tex.LoadImage(getAvatarResult.Data);
                            LoggedUserAvatar = tex;
                        }
                        else
                        {
                            Debug.LogWarning("Failed to load user avatar");
                        }
                    });
                });
            });
        }
        
        public void CheckAuth(Action callback)
        {

            if (IsLoggedOut)
            {
                callback.Invoke();
                return;
            }

            if (m_IsAuthValidated)
            {
                callback.Invoke();
                return;
            }

            AuthCheckInProgress = true;
            var authCheck = new AuthCheck();
            authCheck.Send(result =>
            {
                if (result.IsFailed)
                {
                    LogOut();
                }

                m_IsAuthValidated = true;
                Networks.Clear();
                var listManagedNetworks = new ListManagedNetworks();
                listManagedNetworks.Send(managedNetworksListResult =>
                {
                    if (managedNetworksListResult.IsSucceeded)
                    {
                        Debug.Log(managedNetworksListResult.DataAsText);
                        var networksList = (List<object>) managedNetworksListResult.DataAsDictionary["networks"];
                        foreach (var networkObject in networksList)
                        {
                            var networkDict = networkObject as Dictionary<string, object>;
                            var network = new RoomfulNetwork(networkDict["id"].ToString(), networkDict["fullName"].ToString());
                            Networks.Add(network);
                        }
                    }
                    
                    AuthCheckInProgress = false;
                    callback.Invoke();
                });
            });
        }
    }
}
