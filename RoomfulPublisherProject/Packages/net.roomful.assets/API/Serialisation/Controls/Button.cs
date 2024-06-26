using System;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public struct ButtonClickArgs
    {
        public Button Target;
    }
    
    [RequireComponent(typeof(Animator))]
    public class Button : PropComponent, IRecreatableOnLoad, IPointerDownHandler, IPointerUpHandler
    {
        public enum State {
            None,
            Normal,
            Pressed,
            Disabled
        }
        
        private static readonly int s_pressedParameter = Animator.StringToHash("Pressed");
        private static readonly int s_disabledParameter = Animator.StringToHash("Disabled");
        
        public delegate void ButtonClick(ButtonClickArgs args);
            
        public event ButtonClick OnClick = delegate { };
        
        protected Animator m_animator;

        protected State m_state = State.None;

        protected bool m_isPointerDown;
        protected bool m_isEnabled = true;

        protected void Awake() {
            m_animator = GetComponent<Animator>();
        }

        public virtual void Disable() {
            m_isPointerDown = false;
            m_isEnabled = false;
            
            SetState(State.Disabled);
        }

        public virtual void Enable() {
            m_isEnabled = true;
            SetState(State.Normal);
        }

        protected virtual void SetState(State state) {
            if (m_state == state) return;

            switch (state) {
                case State.Normal:
                    m_animator.SetBool(s_pressedParameter, false);
                    m_animator.SetBool(s_disabledParameter, false);
                    break;
                case State.Disabled:
                    m_animator.SetBool(s_pressedParameter, false);
                    m_animator.SetBool(s_disabledParameter, true);
                    break;
                case State.Pressed:
                    m_animator.SetBool(s_pressedParameter, true);
                    m_animator.SetBool(s_disabledParameter, false);
                    break;
                default: break;
            }
            
            m_state = state;
        }

        public virtual void HandlePointerDown(PointerDownArgs args) {
            if (!m_isEnabled) {
                return;
            }
            
            if (args.Button != InputButton.Left) {
                return;
            }
            
            m_isPointerDown = true;
            SetState(State.Pressed);
        }

        public virtual void HandlePointerUp(PointerUpArgs args) {
            if (!m_isEnabled) {
                return;
            }
            
            if (args.Button != InputButton.Left) {
                return;
            }
            
            if (m_isPointerDown) {
                OnClick(new ButtonClickArgs {
                    Target = this
                });
                
                SetState(m_isEnabled ? State.Normal : State.Disabled);
            }
            m_isPointerDown = false;
        }
    }
}