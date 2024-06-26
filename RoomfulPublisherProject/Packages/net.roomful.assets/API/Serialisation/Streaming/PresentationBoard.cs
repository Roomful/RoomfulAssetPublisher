using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    public class PresentationBoard : PropComponent, IRecreatableOnLoad
    {
        public Canvas UiCanvas;
        public RectTransform UiContainer;
        
        protected virtual void OnDestroy() { }
    }
}