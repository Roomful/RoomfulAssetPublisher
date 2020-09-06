using System.Collections.Generic;
using net.roomful.assets.Network.Request;
using UnityEditor;
using UnityEngine;


namespace net.roomful.assets.Editor
{

    public class BatchReuploadPanel : Panel
    {

        public BatchReuploadPanel(EditorWindow window) : base(window) { }


        public override void OnGUI() {
            if (!BatchDownloadService.IsDownloadingProps) {
                var downloadAllClicked = GUILayout.Button("Download all assets");
                if (downloadAllClicked) {
                    DownloadAllProps();
                } 
            }
            
            var continueUploadClicked = GUILayout.Button("Continue upload");
            if (continueUploadClicked) {
                ContinueUploadProps();
            }
            
            var uploadAllClicked = GUILayout.Button("Upload all assets");
            if (uploadAllClicked) {
                UploadAllProps();
            }
            var checkAuthClicked = GUILayout.Button("Check Auth");
            if (checkAuthClicked) {
                var checkAuth = new CheckAuth();
                checkAuth.PackageCallbackText += s => { Debug.Log(s); }; 
                checkAuth.PackageCallbackError += s => { Debug.Log(s); }; 
                checkAuth.Send();
            }
        }

        private void UploadAllProps() {
            BatchUploadService.UploadAllAssets();
        }

        private void ContinueUploadProps() {
            BatchUploadService.ContinueUpload();
        }
        
        private void DownloadAllProps() {
            BatchDownloadService.DownloadAllAssets();
        }
    }
}