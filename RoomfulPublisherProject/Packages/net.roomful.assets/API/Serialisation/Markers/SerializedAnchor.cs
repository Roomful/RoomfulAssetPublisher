using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedAnchor : MonoBehaviour, IRecreatableOnLoad
    {
        public GameObject Parent;

        [Header("Anchoring")] public Vector3 Anchor = new Vector3(0.5f, 0.5f, 0.5f);
        public Vector3 Offset = Vector3.zero;

        public bool UseRendererPivot = true;
        public Vector3 RendererPivot = new Vector3(0.5f, 0.5f, 0.5f);

        [Header("Size Scale")] public bool EnableXScale = false;
        public float XSize = 1f;

        public bool EnableYScale = false;
        public float YSize = 1f;
    }
}