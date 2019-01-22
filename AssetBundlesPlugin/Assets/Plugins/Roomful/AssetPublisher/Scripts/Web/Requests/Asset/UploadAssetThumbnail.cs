using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
    public class UploadAssetThumbnail : BaseWebPackage {

        private string m_url;
        public UploadAssetThumbnail(string packUrl, Texture2D icon) {
            m_url = packUrl;
            m_packData = icon.EncodeToPNG();
        }

        public override string Url {
            get {
                return m_url;
            }
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.PUT;
            }
        }

        public override bool IsDataPack {
            get {
                return true;
            }
        }

    }
}