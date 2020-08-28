using System;
using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace RF.AssetBundles.Serialization {
    
    [Serializable]
    public class ContainerElement : MonoBehaviour, IRecreatableOnLoad {
        public int ResourceIndex;
    }
}
