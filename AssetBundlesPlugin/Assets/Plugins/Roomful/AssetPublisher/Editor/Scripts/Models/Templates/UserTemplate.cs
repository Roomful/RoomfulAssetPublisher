using System;
using System.Collections.Generic;
using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Network.Request;
using SA.Foundation.Events;
using UnityEngine;

namespace RF.AssetWizzard.Models {

    public class UserTemplate {
        
        public string Id { get; private set; } = string.Empty;
        public string Created { get; private set; }
        public string Updated { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Birthday { get; private set; } = string.Empty;
        public string Hometown { get; private set; } = string.Empty;
        public string Education { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string LinkUrl { get; private set; } = string.Empty;
        public string Avatar { get; private set; } = string.Empty;
        public bool Admin { get; private set; }
        
        private Texture2D m_avatar;
        private SA_Event<Texture2D> m_onAvatarLoaded = new SA_Event<Texture2D>();
        public SA_iEvent<Texture2D> OnAvatarLoaded => m_onAvatarLoaded;
        
        public UserTemplate(IReadOnlyDictionary<string, object> data) {
            if (data.ContainsKey("id")) {
                Id = data["id"].ToString();
            }
            if (data.ContainsKey("created")) {
                Created = data["created"].ToString();
            }
            if (data.ContainsKey("updated")) {
                Updated = data["updated"].ToString();
            }
            if (data.ContainsKey("email")) {
                Email = data["email"].ToString();
            }
            if (data.ContainsKey("phoneNumber")) {
                PhoneNumber = data["phoneNumber"].ToString();
            }
            if (data.ContainsKey("firstName")) {
                FirstName = data["firstName"].ToString();
            }
            if (data.ContainsKey("lastName")) {
                LastName = data["lastName"].ToString();
            }
            if (data.ContainsKey("birthday")) {
                Birthday = data["birthday"].ToString();
            }
            if (data.ContainsKey("hometown")) {
                Hometown = data["hometown"].ToString();
            }            
            if (data.ContainsKey("education")) {
                Education = data["education"].ToString();
            }            
            if (data.ContainsKey("description")) {
                Description = data["description"].ToString();
            }           
            if (data.ContainsKey("linkUrl")) {
                LinkUrl = data["linkUrl"].ToString();
            }            
            if (data.ContainsKey("avatar")) {
                Avatar = data["avatar"].ToString();
                LoadAvatar();
            }
           
            if (data.ContainsKey("permissions")) {
                var permissions = data["permissions"] as Dictionary<string, object>;
                if (permissions.ContainsKey("admin")) {
                    Admin = Convert.ToBoolean(permissions["admin"]);
                }
            }
        }

        //todo Add getter for Avatar
        //todo move this code to new TextureLoader class(should create it).
        
        private void LoadAvatar() {
            GetTextureWWW(GetAvatarResource());
        }

        private Resource GetAvatarResource() {
            var res = new Resource();
            res.SetId(Avatar);

            return res;
        }

        private void GetTextureWWW(Resource res) {
            new GetResourceThumbnailUrlCommand(res.Id).Execute(result => {
                DownloadResource(result.Url);
            });
        }

        private void DownloadResource(string url) {
            var loadResource = new DownloadResource(url) {
                PackageCallbackData = assetData => {
                    var tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    tex.LoadImage (assetData);
                    SetAvatarTexture(tex);
                }
            };
            loadResource.Send();
        }
        
        private void SetAvatarTexture(Texture2D tex) {
            if (tex == null) 
                return;
            
            m_avatar = tex;
            m_onAvatarLoaded.Invoke(tex);
            m_onAvatarLoaded.RemoveAllListeners();
        }
        
        public string FullName() {
            return string.Concat(FirstName, " ", LastName); 
        } 
    }
}