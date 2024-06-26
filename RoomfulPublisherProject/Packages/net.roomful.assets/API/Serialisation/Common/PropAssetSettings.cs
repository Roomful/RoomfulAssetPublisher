using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class PropAssetSettings : MonoBehaviour, IRecreatableOnLoad, IPropAssetSettings
    {
        public bool ShowAnimationsUI;
        public bool AutoRotation;

        public bool ShowAnimationUI => ShowAnimationsUI;
        public bool IsAutoRotation => AutoRotation;
    }
}
