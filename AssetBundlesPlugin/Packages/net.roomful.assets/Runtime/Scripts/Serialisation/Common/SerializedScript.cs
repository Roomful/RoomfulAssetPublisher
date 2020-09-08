using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization {

    public interface IRecreatableOnLoad {
        GameObject gameObject { get; }
    }
}