using System;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public abstract class VerusMusicPlayer : MonoBehaviour, IPropComponent, IRecreatableOnLoad
    {
        [Header("UI Layout")]
        [SerializeField] protected Canvas m_UiCanvas;
        [SerializeField] protected RectTransform m_PlayerControlsContainer;
        [SerializeField] protected RectTransform m_TracksViewContainer;
        
        [Header("Player Buttons")]
        [SerializeField] GameObject m_PrevButton;
        [SerializeField] GameObject m_PlayButton;
        [SerializeField] GameObject m_PauseButton;
        [SerializeField] GameObject m_NextButton;
        
        [Header("Animations")]
        [SerializeField] Animator m_Animator;
        
        public Canvas Canvas => m_UiCanvas;
        public Animator PropAnimator => m_Animator;
        public RectTransform PlayerControlsContainer => m_PlayerControlsContainer;
        public RectTransform TracksViewContainer => m_TracksViewContainer;

        public ActionButton PrevButton { get; private set; }
        public ActionButton PlayButton { get; private set; }
        public ActionButton PauseButton { get; private set; }
        public ActionButton NextButton { get; private set; }
        
        public event Action OnZoomOpen = delegate {  };
        public event Action OnZoomClosed = delegate {  };
        public event Action OnRefresh = delegate {  };
        
        public virtual void Init(IProp prop, int componentIndex)
        {
            PrevButton = m_PrevButton.GetComponentInChildren<ActionButton>();
            PlayButton = m_PlayButton.GetComponentInChildren<ActionButton>();
            PauseButton = m_PauseButton.GetComponentInChildren<ActionButton>();
            NextButton = m_NextButton.GetComponentInChildren<ActionButton>();
        }

        public void OnPropUpdated()
        {
            OnRefresh.Invoke();
        }
        public void PropScaleChanged() {}
        
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
