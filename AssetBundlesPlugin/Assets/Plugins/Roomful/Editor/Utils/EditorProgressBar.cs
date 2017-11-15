using UnityEditor;


namespace RF.AssetWizzard.Editor
{

    public static class EditorProgressBar
    {

        private static float s_uploadProgress = 0f;

        public static void StartUploadProgress(string message) {
            s_uploadProgress = 0f;
            EditorUtility.DisplayProgressBar("Asset Upload", message, s_uploadProgress);
        }

        public static void AddProgress(string message, float progress) {
            s_uploadProgress += progress;
            EditorUtility.DisplayProgressBar("Asset Upload", message, s_uploadProgress);
        }

        public static void FinishUploadProgress() {
            EditorUtility.ClearProgressBar();
        }


        public static float UploadProgress {
            get {
                return s_uploadProgress;
            }

            set {
                s_uploadProgress = value;
            }
        }
    }
}
