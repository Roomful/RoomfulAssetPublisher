using System;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class VariantThumbnailUploader : ThumbnailUploader
    {
        private readonly string m_assetId;
        private readonly IPropVariant m_variant;
        protected override string TemplateTitle => m_variant.Name;

        public VariantThumbnailUploader(string assetId, IPropVariant variant) {
            m_variant = variant;
            m_assetId = assetId;
        }

        protected override void GetUploadLink(Action<string> callbacks) {
            var getIconUploadLink = new GetVariantThumbnailUploadLink(m_assetId, m_variant.Id);
            getIconUploadLink.PackageCallbackText = callbacks;
            getIconUploadLink.Send();
        }

        protected override void ConfirmUpload(Action<Resource> callback) {
            var confirmRequest = new ConfirmVariantThumbnailUpload(m_assetId, m_variant.Id);
            confirmRequest.PackageCallbackText = resData => {
                Debug.Log("result: " + resData);
                var resource = new Resource(resData);
                callback.Invoke(resource);
            };
            confirmRequest.Send();
        }
    }
}