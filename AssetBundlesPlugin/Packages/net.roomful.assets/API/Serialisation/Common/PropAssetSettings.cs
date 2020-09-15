using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class PropAssetSettings : MonoBehaviour, IRecreatableOnLoad, IPropAssetSettings
    {
        [SerializeField]
        private bool m_showAnimationsUI = default;

        public bool ShowAnimationUI => m_showAnimationsUI;
    }
}