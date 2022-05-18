using System.Collections.Generic;

namespace net.roomful.api
{
    public class Avatar3DInfo
    {
        public string AssetId { get; } = string.Empty;
        private readonly Dictionary<string, string> m_skins;

        public IReadOnlyDictionary<string, string> Skins => m_skins;

        public Avatar3DInfo(string assetId, Dictionary<string, string> skins) {
            AssetId = assetId;
            m_skins = skins;
        }

        public Avatar3DInfo(JSONData data) {
            if (data.HasValue("avatar3D")) {
                var jsonData = data.GetValue<Dictionary<string, object>>("avatar3D");
                var avatarData = new JSONData(jsonData);
                AssetId = avatarData.GetValue<string>("assetId");

                if (avatarData.HasValue("assetSkins")) {
                    m_skins = new Dictionary<string, string>();
                    var skins = avatarData.GetValue<Dictionary<string, object>>("assetSkins");

                    foreach (var skin in skins) {
                        m_skins.Add(skin.Key, skin.Value.ToString());
                    }
                }
            }
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