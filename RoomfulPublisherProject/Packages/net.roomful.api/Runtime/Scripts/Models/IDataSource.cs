using System;
using net.roomful.api;
using net.roomful.api.assets;
using net.roomful.api.sa;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.models
{
    /// <summary>
    /// Basic Data source implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataSource<T> where T : ITemplate
    {
        /// <summary>
        /// Event called when item is inserted.
        /// </summary>
        event Action<T> OnItemInserted;

        /// <summary>
        /// Event called when item is updated.
        /// </summary>
        event Action<T> OnItemUpdated;

        /// <summary>
        /// Event called when item is removed.
        /// </summary>
        event Action<T> OnItemRemoved;

        /// <summary>
        /// Event called when item is cleared.
        /// </summary>
        event Action OnItemsCleared;
    }
}
