using System;
using System.Collections.Generic;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.api
{
    public class UserDataModelSimple : TemplateDataModel
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public Texture2D Avatar { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public int Counter { get; }

        public string CompanyName { get; set; } = string.Empty;
        public string CompanyTitle { get; set; } = string.Empty;
        public Avatar3DInfo Avatar3DInfo { get; set; }
        public UserPrivacyMode UserPrivacyMode { get; set; } = UserPrivacyMode.Default;

        public UserDataModelSimple() {
            // For anonymous user.
            Avatar3DInfo = new Avatar3DInfo(string.Empty, new Dictionary<string, string>());
        }

        public UserDataModelSimple(JSONData userInfo) : base(userInfo) {
            FirstName = userInfo.GetValue<string>("firstName");
            LastName = userInfo.GetValue<string>("lastName");
            Avatar3DInfo = new Avatar3DInfo(userInfo);
            if (userInfo.HasValue("avatarBase64")) {
                var avatarBase64 = userInfo.GetValue<string>("avatarBase64");
                if (avatarBase64 != string.Empty) {
                    var decodedFromBase64 = Convert.FromBase64String(avatarBase64);
                    Avatar = new Texture2D(1, 1, TextureFormat.RGB24, false);
                    Avatar.LoadImage(decodedFromBase64);
                }
            }

            if (userInfo.HasValue("avatar")) {
                AvatarUrl = userInfo.GetValue<string>("avatar");
            }

            if (userInfo.HasValue("counter")) {
                Counter = userInfo.GetValue<int>("counter");
            }


            if (userInfo.HasValue("companyName")) {
                CompanyName = userInfo.GetValue<string>("companyName");
            }

            if (userInfo.HasValue("companyTitle")) {
                CompanyTitle = userInfo.GetValue<string>("companyTitle");
            }

            if (userInfo.HasValue("privacyMode")) {
                UserPrivacyMode = (UserPrivacyMode)userInfo.GetValue<int>("privacyMode");
            }
        }

        public Dictionary<string, object> ToDictionary(bool includeBase64 = true) {
            var data = new Dictionary<string, object>();
            data.Add("id", Id);
            data.Add("firstName", FirstName);
            data.Add("lastName", LastName);
            data.Add("avatar", AvatarUrl);
            if (includeBase64 && Avatar != null) {
                var bytes = Avatar.EncodeToPNG();
                var avatarBase64 = Convert.ToBase64String(bytes);
                data.Add("avatarBase64", avatarBase64);
            }
            else {
                data.Add("avatarBase64", string.Empty);
            }
            data.Add("privacyMode", (int)UserPrivacyMode);

            return data;
        }

        public string GetShortenName() {
            string firstName;
            var lastName = LastName;

            if (FirstName.Contains(" ")) {
                var namesArray = FirstName.Split(' ');
                firstName = namesArray[0];
            }
            else {
                firstName = FirstName;
            }

            var tempName = firstName;
            if (!string.IsNullOrEmpty(lastName) && lastName.Length > 0) {
                tempName = string.Concat(firstName, " ", lastName.Substring(0, 1));
            }

            return tempName;
        }
    }
}
