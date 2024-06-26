using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class UploadAsset_Thumbnail : BaseWebPackage
    {
        private const RequestMethods PackMethodName = RequestMethods.PUT;

        public UploadAsset_Thumbnail(string packUrl, Texture2D icon) : base(packUrl, PackMethodName) {
            m_PackData = icon.EncodeToPNG();
        }

        public override bool IsDataPack => true;

        public override Dictionary<string, object> GenerateData() {
            var originalJSON = new Dictionary<string, object>();

            return originalJSON;
        }
    }
}