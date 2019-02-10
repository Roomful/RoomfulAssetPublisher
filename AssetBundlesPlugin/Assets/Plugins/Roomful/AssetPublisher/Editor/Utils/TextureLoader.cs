using System;
using RF.AssetWizzard.Commands;
using RF.AssetWizzard.Network.Request;
using UnityEngine;

namespace RF.AssetWizzard.Editor {
    
    public static class TextureLoader {
        
        public static void GetTextureWWW(string resId, Action<Texture2D> callback) {
            GetTextureWWW(GetAvatarResource(resId), callback);
        }

        private static Resource GetAvatarResource(string resId) {
            var res = new Resource();
            res.SetId(resId);
            return res;
        }
        
        public static void GetTextureWWW(Resource res, Action<Texture2D> callback) {
            new GetResourceThumbnailUrlCommand(res.Id).Execute(result => {
                if (string.IsNullOrEmpty(result.Url)) {
                    callback.Invoke(null);
                    return;
                }
                DownloadResource(result.Url, callback);
            });
        }

        private static void DownloadResource(string url, Action<Texture2D> callback) {
            var loadResource = new DownloadResource(url) {
                PackageCallbackData = assetData => {
                    var tex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
                    tex.LoadImage (assetData);
                    callback.Invoke(tex);
                }
            };
            loadResource.Send();
        }
    }
}
