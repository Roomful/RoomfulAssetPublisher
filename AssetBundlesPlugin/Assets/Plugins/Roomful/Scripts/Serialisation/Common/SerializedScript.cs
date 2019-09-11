using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace RF.AssetBundles.Serialization {

    public interface IRecreatableOnLoad {
        GameObject gameObject { get; }
    }
}