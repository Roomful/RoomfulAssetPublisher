using System;
using net.roomful.api.props;
using UnityEngine;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.assets.serialization
{
    [Serializable]
    [AddComponentMenu("Roomful/Container Section Focus")]
    public class SerializedSectionFocusPointer : PropComponent, IRecreatableOnLoad
    {
        public int Id = 0;
    }
}
