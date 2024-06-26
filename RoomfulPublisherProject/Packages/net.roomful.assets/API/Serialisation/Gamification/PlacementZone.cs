using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class PlacementZone : MonoBehaviour, IPropComponent, IRecreatableOnLoad
    {
        public void Init(IProp prop, int componentIndex) { }
        public void OnPropUpdated() { }
        public void PropScaleChanged() { }
        public void OnZoomViewOpen() { }
        public void OnZoomViewClosed() { }
    }
}
