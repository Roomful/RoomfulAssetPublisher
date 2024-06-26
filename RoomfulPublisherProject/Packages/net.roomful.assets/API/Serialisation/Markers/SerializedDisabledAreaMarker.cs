using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Disabled Area Marker")]
    public class SerializedDisabledAreaMarker : MonoBehaviour, IRecreatableOnLoad
    {
        private void Start() {
            if (Roomful.Assets != null) {
                Roomful.Assets.MarkAsDisableArea(gameObject);
                Destroy(this);
            }
        }
    }
}
