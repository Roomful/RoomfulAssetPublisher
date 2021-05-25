using System;
using UnityEngine;
using SA.Android.Gallery;
using SA.Foundation.Templates;

namespace SA.CrossPlatform.App
{
    class UM_AndroidGalleryService : UM_AbstractGalleryService, UM_iGalleryService
    {
        public void PickImage(int thumbnailSize, Action<UM_MediaResult> callback)
        {
            PickMedia(thumbnailSize, AN_MediaType.Image, callback);
        }

        public void PickVideo(int thumbnailSize, Action<UM_MediaResult> callback)
        {
            PickMedia(thumbnailSize, AN_MediaType.Video, callback);
        }

        void PickMedia(int thumbnailSize, AN_MediaType type, Action<UM_MediaResult> callback)
        {
            var picker = new AN_MediaPicker(type);
            picker.AllowMultiSelect = false;
            picker.MaxSize = thumbnailSize;

            picker.Show(result =>
            {
                UM_MediaResult pickResult;
                if (result.IsSucceeded)
                {
                    UM_MediaType umType;
                    switch (type)
                    {
                        case AN_MediaType.Video:
                            umType = UM_MediaType.Video;
                            break;
                        default:
                            umType = UM_MediaType.Image;
                            break;
                    }

                    result.Media[0].GetThumbnailAsync(texture2D =>
                    {
                        var media = new UM_Media(texture2D, result.Media[0].Path, umType);
                        pickResult = new UM_MediaResult(media);
                        callback.Invoke(pickResult);
                    });
                }
                else
                {
                    pickResult = new UM_MediaResult(result.Error);
                    callback.Invoke(pickResult);
                }
            });
        }

        public override void SaveImage(Texture2D image, string fileName, Action<SA_Result> callback)
        {
            AN_Gallery.SaveImageToGallery(image, fileName, callback.Invoke);
        }
    }
}
