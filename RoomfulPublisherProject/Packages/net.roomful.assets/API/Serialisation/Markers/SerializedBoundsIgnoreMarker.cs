using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Bounds Ignore Marker")]
    public class SerializedBoundsIgnoreMarker : MonoBehaviour, IRecreatableOnLoad
    {
        private void Start() {
            if (Roomful.Assets != null) {
                Roomful.Assets?.IgnoreBounds(gameObject);
                Destroy(this);
            }
        }
    }
}
