using UnityEditor;

namespace net.roomful.assets.editor
{
    internal static class MessageUtils
    {
        public static void ShowNotification(string message) {
            EditorUtility.DisplayDialog("Note", message, "Ok");
        }
    }
}