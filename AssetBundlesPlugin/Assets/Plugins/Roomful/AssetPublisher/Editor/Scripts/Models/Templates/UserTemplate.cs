using System;
using System.Collections.Generic;
using RF.AssetWizzard.Editor;
using SA.Foundation.Events;
using UnityEngine;

namespace RF.AssetWizzard.Models {

    [Serializable]
    public class UserTemplate {

        public string Id               { get; } = string.Empty;
        public string Created          { get; } = string.Empty;
        public string Updated          { get; } = string.Empty;
        public string Email            { get; } = string.Empty;
        public string PhoneNumber      { get; } = string.Empty;
        public string FirstName        { get; } = string.Empty;
        public string LastName         { get; } = string.Empty;
        public string Birthday         { get; } = string.Empty;
        public string Hometown         { get; } = string.Empty;
        public string Education        { get; } = string.Empty;
        public string Description      { get; } = string.Empty;
        public string LinkUrl          { get; } = string.Empty;
        public string AvatarResourceId { get; } = string.Empty;
        public bool Admin              { get; }
        
        private Texture2D m_avatar;
        
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
                AvatarResourceId = data["avatar"].ToString();
            }

            if (!data.ContainsKey("permissions")) 
                return;
            var permissions = data["permissions"] as Dictionary<string, object>;
            if (permissions.ContainsKey("admin")) {
                Admin = Convert.ToBoolean(permissions["admin"]);
            }
        }

        public string FullName() {
            return string.Concat(FirstName, " ", LastName);
        }

        public void GetAvatar(Action<Texture2D> callback) {
            if (string.IsNullOrEmpty(AvatarResourceId)) {
                callback.Invoke(new Texture2D(1, 1));
                return;
            }
            
            if (m_avatar != null) {
                callback.Invoke(m_avatar);
                return;
            }

            LoadAvatar(callback);
        }
        
        private void LoadAvatar(Action<Texture2D> callback) {
            TextureLoader.GetTextureWWW(AvatarResourceId, texture => {
                m_avatar = texture;
                callback.Invoke(m_avatar.Equals(null) ? new Texture2D(1, 1) : m_avatar);
            });
        }
    }
}