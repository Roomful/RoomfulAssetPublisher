using UnityEditor;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{

    public abstract class Panel : WizzardUIComponent, IPanel
    {

       
        private readonly EditorWindow m_window;

        public Panel(EditorWindow window) {
            m_window = window;
        }


        public abstract void OnGUI();


        public EditorWindow Window {
            get {
                return m_window;
            }
        }

        public virtual bool CanBeSelected {
            get {
                return true;
            }
        }


       
    }
}