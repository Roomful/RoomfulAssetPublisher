using UnityEngine;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    class ApplyHideFlags : MonoBehaviour
    {
        public HideFlags flag;

        // Use this for initialization
        void Awake() {
#if UNITY_EDITOR
            UnityEditor.SceneVisibilityManager.instance.DisablePicking(gameObject, true);
#endif
            gameObject.hideFlags = flag;
        }
    }
}