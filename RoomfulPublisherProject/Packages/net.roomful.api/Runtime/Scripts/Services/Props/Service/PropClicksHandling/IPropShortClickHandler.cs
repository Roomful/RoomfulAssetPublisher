using System;
using UnityEngine;

namespace net.roomful.api.props
{
    public enum PropagationState
    {
        None,
        Active,
        Consumed,
        Stopped
    }

    public interface IClickEvent
    {
        PropagationState State { get; }

        void StopPropagation();
    }
    
    public interface IShortClickEvent : IClickEvent
    {
        PropClickContext Context { get; }
    }
    
    public class ShortClickEvent : BaseEvent<PropClickContext>, IShortClickEvent
    {
        
    }

    public abstract class BaseEvent<T> : IDisposable
    {
        public T Context { get; private set; }
        public PropagationState State { get; private set; }

        public void Bind(T ctx)
        {
            Context = ctx;
            State = PropagationState.Active;
        }

        public void Dispose()
        {
            Context = default;
            State = PropagationState.None;
        }

        public void Consume()
        {
            State = PropagationState.Consumed;
        }
        
        public void StopPropagation()
        {
            State = PropagationState.Stopped;
        }
    }
    
    public struct PropClickContext
    {
        public IProp Prop;
        public TouchEventData TouchEventData;
        public RaycastHit Hit;
    }
    
    public interface IPropShortClickHandler
    {
        void OnPropShortClicked(IShortClickEvent e);
    }
}
