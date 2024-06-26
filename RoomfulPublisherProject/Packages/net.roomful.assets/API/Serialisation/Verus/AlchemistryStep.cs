using System;
using System.Collections.Generic;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    /// <summary>
    /// Universal Alchemistry Step component.
    /// </summary>
    public class AlchemistryStep : MonoBehaviour, IPropComponent, IRecreatableOnLoad
    {
        public event Action OnZoomOpen = delegate {  };
        public event Action OnZoomClosed = delegate {  };

        [SerializeField] string m_StepId;
        [SerializeField] Canvas m_Canvas;
        [SerializeField] List<RectTransform> m_UIRects;
        [SerializeField] List<GameObject> m_Objects;
        
        public Canvas Canvas => m_Canvas;
        public string StepId => m_StepId;
        public List<RectTransform> UIRects => m_UIRects;
        public List<GameObject> Objects => m_Objects;
        
        public void Init(IProp prop, int componentIndex)
        {
            
        }

        public void OnPropUpdated()
        {
            
        }

        public void PropScaleChanged()
        {
            
        }

        public void OnZoomViewOpen()
        {
            OnZoomOpen.Invoke();
        }

        public void OnZoomViewClosed()
        {
            OnZoomClosed.Invoke();
        }
    }
}