using System.Collections.Generic;

namespace net.roomful.api
{
    public class Avatar3DInfo
    {
        public string AssetId => "3p23d1mxp16xh3"; // Hardcoded because of new RPM Avatars
        public string AvatarUrl { get; private set; }
        private readonly Dictionary<string, string> m_skins;

        public IReadOnlyDictionary<string, string> Skins => m_skins;
        
        public bool EnableBlendShapes { get; set;  }
        
        private static readonly Dictionary<string, string> s_emptyDictionary = new Dictionary<string, string>();

        public Avatar3DInfo() {
            m_skins = s_emptyDictionary;
        }
        
        public Avatar3DInfo(string avatarUrl, bool enableBlendShapes = false) : this() {
            AvatarUrl = avatarUrl;
            EnableBlendShapes = enableBlendShapes;
        }
        
        // TODO: Remove, it is redundant
        public Avatar3DInfo(string assetId, Dictionary<string, string> skins) {
            m_skins = skins;
        }

        public Avatar3DInfo(JSONData data) {
            if (data.HasValue("avatar3D")) {
                EnableBlendShapes = true;
                var jsonData = data.GetValue<Dictionary<string, object>>("avatar3D");
                var avatarData = new JSONData(jsonData);

                // Commented out because we don't need skins anymore
                // if (avatarData.HasValue("assetSkins")) {
                //     m_skins = new Dictionary<string, string>();
                //     var skins = avatarData.GetValue<Dictionary<string, object>>("assetSkins");
                //
                //     foreach (var skin in skins) {
                //         m_skins.Add(skin.Key, skin.Value.ToString());
                //     }
                // }

                if (avatarData.HasValue("avatarUrl")) {
                    AvatarUrl = avatarData.GetValue<string>("avatarUrl");
                }
            }
        }

        public void Update(Avatar3DInfo otherInfo) {
            AvatarUrl = otherInfo.AvatarUrl;
        }

        public bool HasTheSameDataAs(Avatar3DInfo other) {
            if (AssetId.Equals(other.AssetId)) {
                return false;
            }

            if (Skins == null) {
                return other.Skins == null;
            }

            if (other.Skins == null) {
                return false;
            }

            if (other.Skins.Count != Skins.Count) {
                return false;
            }

            foreach (var skin in Skins) {

                var key = skin.Key;
                var val = skin.Value;
                if (other.Skins.TryGetValue(key, out var otherVal)) {
                    if (!val.Equals(otherVal)) {
                        return false;
                    }
                }
                else {
                    return false;
                }

            }

            return true;
        }
    }
}
