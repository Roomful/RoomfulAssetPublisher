using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    [ExecuteInEditMode]
    [AddComponentMenu("Roomful/Floor Marker")]
    public class SerializedFloorMarker : MonoBehaviour, IRecreatableOnLoad
    {
        private void Start() {
            if (Roomful.Assets != null) {
                Roomful.Assets.MarkAsStandArea(gameObject);
                Destroy(this);
            }
        }
    }
}
