using UnityEngine;

// Copyright Roomful 2013-2019. All rights reserved.

namespace net.roomful.assets.serialization
{
    /// <summary>
    /// If you use custom component, you should use this interface, or otherwise your component will be lost
    /// when re-uploading an asset.
    /// 
    /// Warning: Do not reference another 'IRecreatableOnLoad' components from your custom component
    /// those references will become missing once asset bundle is downloaded.
    /// </summary>
    public interface IRecreatableOnLoad
    {
        // ReSharper disable once InconsistentNaming
        GameObject gameObject { get; }
    }
}