using System;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class RoomTunnel : MonoBehaviour, IPropComponent, IRecreatableOnLoad
    {
        [SerializeField] float m_DefaultActivationDistance = 2f;
        public float DefaultActivationDistance => m_DefaultActivationDistance;
        
        public event Action OnZoomOpen = delegate {  };
        public event Action OnZoomClosed = delegate {  };
        public event Action OnPropUpdate = delegate {  };
        
        public void Init(IProp prop, int componentIndex)
        {
            
        }

        public void OnPropUpdated()
        {
            OnPropUpdate?.Invoke();
        }

        public void PropScaleChanged()
        {
        }

        public void OnZoomViewOpen()
        {
            OnZoomOpen?.Invoke();
        }

        public void OnZoomViewClosed()
        {
            OnZoomClosed?.Invoke();
        }
    }
}
