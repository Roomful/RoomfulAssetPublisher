using System;

namespace net.roomful.assets.editor
{
    internal class AssetThumbnailUploader : ThumbnailUploader
    {
        private readonly AssetTemplate m_assetTemplate;
        protected override string TemplateTitle => m_assetTemplate.Title;

        protected override void GetUploadLink(Action<string> callbacks) {
            var getIconUploadLink = new GetAssetThumbnailUploadLink(m_assetTemplate.Id);
            getIconUploadLink.PackageCallbackText = callbacks;
            getIconUploadLink.Send();
        }

        protected override void ConfirmUpload(Action<Resource> callback) {
            var confirmRequest = new ConfirmAssetThumbnailUpload(m_assetTemplate.Id);
            confirmRequest.PackageCallbackText = resData => {
                var resource = new Resource(resData);
                callback.Invoke(resource);
            };
            confirmRequest.Send();
        }

        public AssetThumbnailUploader(AssetTemplate assetTemplate) {
            m_assetTemplate = assetTemplate;
        }
    }
}