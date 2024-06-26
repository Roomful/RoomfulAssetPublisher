using net.roomful.api.props;
using net.roomful.assets;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.api.props
{
    public abstract class PropComponent : MonoBehaviour, IPropComponent
    {
        protected IProp Prop { get; private set; }
        protected int ComponentIndex { get; private set; }
        
        
        public virtual void Init(IProp prop, int componentIndex) {
            ComponentIndex = componentIndex;
            Prop = prop;
        }

        public virtual void OnPropUpdated() { }
        protected virtual void OnPropScaleChanged() { }
        

        public void PropScaleChanged() {
            OnPropScaleChanged();
        }

        public virtual void OnZoomViewOpen()
        {
            
        }

        public virtual void OnZoomViewClosed()
        {

        }
    }
}
