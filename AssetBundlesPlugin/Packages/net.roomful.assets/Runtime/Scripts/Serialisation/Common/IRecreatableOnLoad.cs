using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization
{
    public interface IRecreatableOnLoad
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }
    }
}