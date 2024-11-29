using System;
using System.Collections.Generic;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class AttestationProp : MonoBehaviour, IPropComponent, IRecreatableOnLoad
    {
        
        public enum AttestationBindingType
        {
            Education,
            WorkExperience,
            Skill,
            Statement,
            IdentityProof,
            SocialNetwork
        }
        
        public event Action OnZoomOpen = delegate {  };
        public event Action OnZoomClosed = delegate {  };
        
        [SerializeField] List<AttestationBindingType> m_BindingType;

        [Header("Data Display")]
        [SerializeField] TextMesh m_MainTittle;
        [SerializeField] TextMesh m_SignedBy;
        [SerializeField] TextMesh m_SignaturesList;
        [SerializeField] TextMesh m_SignaturesCount;
        
        [Header("Education, WorkExperience only")]
        [SerializeField] TextMesh m_DateStart;
        [SerializeField] TextMesh m_DateEnd;
        [SerializeField] TextMesh m_Description;
        
        public TextMesh MainTittle => m_MainTittle;
        public TextMesh SignedBy => m_SignedBy;
        public TextMesh SignaturesList => m_SignaturesList;
        public TextMesh SignaturesCount => m_SignaturesCount;
        
        public TextMesh DateStart => m_DateStart;
        public TextMesh DateEnd => m_DateEnd;
        public TextMesh Description => m_Description;
        
        public List<AttestationBindingType> BindingType => m_BindingType;
        
        public void Init(IProp prop, int componentIndex) { }
        public void OnPropUpdated() { }
        public void PropScaleChanged() { }

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
