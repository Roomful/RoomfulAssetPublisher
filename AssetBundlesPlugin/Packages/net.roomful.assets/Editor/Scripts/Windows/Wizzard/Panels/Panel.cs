using UnityEditor;

namespace net.roomful.assets.Editor
{

    public abstract class Panel : WizzardUIComponent, IPanel
    {

       
        private readonly EditorWindow m_window;

        public Panel(EditorWindow window) {
            m_window = window;
        }


        public abstract void OnGUI();


        public EditorWindow Window => m_window;

        public virtual bool CanBeSelected => true;
    }
}