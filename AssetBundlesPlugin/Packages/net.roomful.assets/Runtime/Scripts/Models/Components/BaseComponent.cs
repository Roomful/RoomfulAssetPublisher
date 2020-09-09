using UnityEngine;

namespace net.roomful.assets
{
    public class BaseComponent : MonoBehaviour
    {
        protected PropAsset Prop => FindObjectOfType<PropAsset>();

        public void RemoveSilhouette() { }
    }
}