using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal static class EditorProgressBar
    {
        public static void AddProgress(string title, string message, float progress) {
            UploadProgress += progress;
            EditorUtility.DisplayProgressBar("Asset " + title + " Upload", message, UploadProgress);
        }

        public static void FinishUploadProgress() {
            EditorUtility.ClearProgressBar();
        }

        public static float UploadProgress { get; set; } = 0f;
    }
}