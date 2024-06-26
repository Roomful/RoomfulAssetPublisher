using UnityEditor;

namespace net.roomful.assets.editor
{
    [CanEditMultipleObjects, CustomEditor(typeof(RoomfulText))]
    internal class TextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();
        }
    }
}