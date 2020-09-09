using UnityEngine;

namespace net.roomful.assets
{
    [ExecuteInEditMode]
    internal class ApplyHideFlags : MonoBehaviour
    {
        public HideFlags flag;

        // Use this for initialization
        void Awake() {
            gameObject.hideFlags = flag;
        }
    }
}