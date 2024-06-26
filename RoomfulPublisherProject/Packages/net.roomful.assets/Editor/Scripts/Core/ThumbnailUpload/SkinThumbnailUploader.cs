using System;

namespace net.roomful.assets.editor
{
    internal class SkinThumbnailUploader : ThumbnailUploader
    {
        private readonly string m_assetId;
        private readonly IPropSkin m_skin;
        protected override string TemplateTitle => m_skin.Name;

        public SkinThumbnailUploader(string assetId, IPropSkin skin) {
            m_skin = skin;
            m_assetId = assetId;
        }

        protected override void GetUploadLink(Action<string> callbacks) {
            var getIconUploadLink = new GetSkinThumbnailUploadLink(m_assetId, m_skin.Id);
            getIconUploadLink.PackageCallbackText = callbacks;
            getIconUploadLink.Send();
        }

        protected override void ConfirmUpload(Action<Resource> callback) {
            var confirmRequest = new ConfirmSkinThumbnailUpload(m_assetId, m_skin.Id);
            confirmRequest.PackageCallbackText = resData => {
                var resource = new Resource(resData);
                callback.Invoke(resource);
            };
            confirmRequest.Send();
        }
    }
}