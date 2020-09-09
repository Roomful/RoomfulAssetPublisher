using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal class SeparatorPanel : Panel
    {
        public SeparatorPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() { }

        public override bool CanBeSelected => false;
    }
}