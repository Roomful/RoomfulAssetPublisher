using UnityEngine;

namespace net.roomful.assets
{
    class BaseComponent : MonoBehaviour
    {
        protected PropAsset Prop => FindObjectOfType<PropAsset>();
    }
}