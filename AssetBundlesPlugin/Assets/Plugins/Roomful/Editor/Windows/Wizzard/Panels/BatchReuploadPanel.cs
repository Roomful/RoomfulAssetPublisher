using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class BatchReuploadPanel : Panel
    {

        public BatchReuploadPanel(EditorWindow window) : base(window) { }


        public override void OnGUI() {
            if (!BatchUploadService.IsDownloadingProps) {
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
        }

        private void UploadAllProps() {
            BatchUploadService.UploadAllAssets();
        }

        private void ContinueUploadProps() {
            BatchUploadService.ContinueUpload();
        }
        
        private void DownloadAllProps() {
            BatchUploadService.DownloadAllAssets();
        }
    }
}