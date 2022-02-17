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
        SA_iSafeEvent<T> OnItemInserted { get; }

        /// <summary>
        /// Event called when item is updated.
        /// </summary>
        SA_iSafeEvent<T> OnItemUpdated { get; }

        /// <summary>
        /// Event called when item is removed.
        /// </summary>
        SA_iSafeEvent<T> OnItemRemoved { get; }

        /// <summary>
        /// Event called when item is cleared.
        /// </summary>
        SA_iSafeEvent OnItemsCleared { get; }
    }
}
