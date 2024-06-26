using System;
using net.roomful.api.props;
using UnityEngine;

namespace net.roomful.assets.serialization
{
    
    [Serializable]
    [AddComponentMenu("Roomful/Container Section Focus Click Area")]
    public class SerializedFocusPointerClickArea: PropComponent, IRecreatableOnLoad
    {
        public int Id = 0;
    }
}
