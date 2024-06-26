using UnityEditor;

namespace net.roomful.assets.editor
{
    class SeparatorPanel : Panel
    {
        public SeparatorPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() { }

        public override bool CanBeSelected => false;
    }
}