using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class SerializedCustomizableSkinGameObject : MonoBehaviour
    {
        [SerializeField] CustomizableSkinGameObjectType m_type;

        public CustomizableSkinGameObjectType Type {
            get => m_type;
            set => m_type = value;
        }
    }
}
