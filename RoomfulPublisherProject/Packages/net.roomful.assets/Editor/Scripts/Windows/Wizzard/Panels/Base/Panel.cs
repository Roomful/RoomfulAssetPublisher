using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    abstract class Panel : WizardUIComponent, IPanel
    {
        protected Panel(EditorWindow window) {
            Window = window;
        }

        public abstract void OnGUI();

        protected EditorWindow Window { get; }

        public virtual bool CanBeSelected
        {
            get
            {
                if (RequireAuth && AssetBundlesSettings.Instance.IsLoggedOut)
                {
                    return false;
                }

                return true;
            }
        }

        protected virtual bool RequireAuth => true;
        protected bool RequireValidPlatform => true;
    }
}