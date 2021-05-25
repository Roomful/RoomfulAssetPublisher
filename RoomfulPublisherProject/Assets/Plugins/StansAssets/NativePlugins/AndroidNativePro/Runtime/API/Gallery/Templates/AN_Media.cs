using System;
using System.Collections;
using JetBrains.Annotations;
using StansAssets.Foundation.Async;
using UnityEngine;
using UnityEngine.Networking;

namespace SA.Android.Gallery
{
    /// <summary>
    /// Picked image from gallery representation
    /// </summary>
    [Serializable]
    public class AN_Media
    {
        [SerializeField]
        string m_path = null;
        [SerializeField]
        AN_MediaType m_type = AN_MediaType.Image;

        const string k_URLFilePathPrefix = "file://";


        /// <summary>
        /// The image path
        /// </summary>
        /// <value>The path.</value>
        public string Path => m_path;


        public void GetThumbnailAsync([NotNull] Action<Texture2D> imageLoadCallback) {
            if (imageLoadCallback == null) {
                throw new ArgumentNullException(nameof(imageLoadCallback));
            }

            if (string.IsNullOrEmpty(m_path)) {
                imageLoadCallback.Invoke(null);
            }
            else {
                CoroutineUtility.Start(LoadIntoTexture(m_path, imageLoadCallback));
            }
        }

        static IEnumerator LoadIntoTexture(string absolutePath, Action<Texture2D> callback) {
            var adjustedPath = k_URLFilePathPrefix + absolutePath;
            using (var request = UnityWebRequestTexture.GetTexture(adjustedPath)) {
                yield return request.SendWebRequest();
                if (!request.isNetworkError && !request.isHttpError) {
                    callback.Invoke(DownloadHandlerTexture.GetContent(request));
                }
                else
                {
                    callback.Invoke(null);
                }
            }
        }

        public AN_MediaType Type => m_type;
    }
}
