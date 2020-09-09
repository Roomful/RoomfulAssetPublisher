using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal abstract class Panel : WizzardUIComponent, IPanel
    {
        protected Panel(EditorWindow window) {
            Window = window;
        }

        public abstract void OnGUI();

        protected EditorWindow Window { get; }

        public virtual bool CanBeSelected => true;
    }
}