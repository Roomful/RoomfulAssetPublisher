using System;
using SA.Android.Camera;

namespace SA.CrossPlatform.App
{
    class UM_AndroidCameraService : UM_iCameraService
    {
        public void TakePicture(int thumbnailSize, Action<UM_MediaResult> callback)
        {
            AN_Camera.CaptureImage(thumbnailSize, result =>
            {
                UM_MediaResult pickResult;
                if (result.IsSucceeded)
                {
                    result.Media.GetThumbnailAsync(texture2D =>
                    {
                        var media = new UM_Media(texture2D, result.Media.Path, UM_MediaType.Image);
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

        public void TakeVideo(int thumbnailSize, Action<UM_MediaResult> callback)
        {
            AN_Camera.CaptureVideo(thumbnailSize, result =>
            {
                UM_MediaResult pickResult;
                if (result.IsSucceeded)
                {
                    result.Media.GetThumbnailAsync(texture2D =>
                    {
                        var media = new UM_Media(texture2D, result.Media.Path, UM_MediaType.Image);
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
    }
}
