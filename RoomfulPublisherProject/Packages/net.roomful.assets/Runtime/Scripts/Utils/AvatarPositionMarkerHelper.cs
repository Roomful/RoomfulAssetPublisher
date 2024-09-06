using UnityEngine;

namespace net.roomful.assets.editor
{
    public class AvatarPositionMarkerHelper : MonoBehaviour, IPropPublihserComponent
    {
        public void PrepareForUpload() {
            Destroy(gameObject);
        }
        
        public void Update() { }

        public PropComponentUpdatePriority UpdatePriority { get; } = PropComponentUpdatePriority.Lowest;
    }
}
