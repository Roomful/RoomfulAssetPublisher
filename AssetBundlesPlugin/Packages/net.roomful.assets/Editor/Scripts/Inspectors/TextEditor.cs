using UnityEditor;

namespace net.roomful.assets.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(RoomfulText))]
    public class TextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}