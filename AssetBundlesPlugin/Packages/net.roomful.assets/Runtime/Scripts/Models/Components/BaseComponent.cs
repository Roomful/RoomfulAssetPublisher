using UnityEngine;

namespace net.roomful.assets
{
    internal class BaseComponent : MonoBehaviour
    {
        protected PropAsset Prop => FindObjectOfType<PropAsset>();

        public void RemoveSilhouette() { }
    }
}