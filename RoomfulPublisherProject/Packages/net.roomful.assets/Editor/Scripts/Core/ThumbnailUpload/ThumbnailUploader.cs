using System;
using UnityEngine;

namespace net.roomful.assets.editor
{
    internal abstract class ThumbnailUploader
    {
        protected abstract string TemplateTitle { get; }

        protected abstract void GetUploadLink(Action<string> callbacks);
        protected abstract void ConfirmUpload(Action<Resource> callbacks);

        public void Upload(Texture2D image, Action<Resource> callback) {
            EditorProgressBar.AddProgress(TemplateTitle, "Requesting Thumbnail Upload Link", 0.1f);
            GetUploadLink(linkCallback => {
                EditorProgressBar.AddProgress(TemplateTitle, "Uploading Asset Thumbnail", 0.1f);
                var uploadRequest = new UploadAsset_Thumbnail(linkCallback, image);

                var currentUploadProgress = EditorProgressBar.UploadProgress;
                uploadRequest.UploadProgress = progress => {
                    var p = progress / 2f;
                    EditorProgressBar.UploadProgress = currentUploadProgress + p;
                    EditorProgressBar.AddProgress(TemplateTitle, "Uploading Asset Thumbnail", 0f);
                };

                uploadRequest.PackageCallbackText = uploadCallback => {
                    EditorProgressBar.AddProgress(TemplateTitle, "Waiting Thumbnail Upload Confirmation", 0.3f);
                    ConfirmUpload(resource => {
                        EditorProgressBar.FinishUploadProgress();
                        callback.Invoke(resource);
                    });
                };
                uploadRequest.Send();
            });
        }
    }
}