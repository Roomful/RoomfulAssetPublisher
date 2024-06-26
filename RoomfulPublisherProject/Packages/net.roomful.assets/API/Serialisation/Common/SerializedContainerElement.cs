using System;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization
{
    [Serializable]
    public class SerializedContainerElement : MonoBehaviour, IRecreatableOnLoad
    {
        public int ResourceIndex;
    }
}