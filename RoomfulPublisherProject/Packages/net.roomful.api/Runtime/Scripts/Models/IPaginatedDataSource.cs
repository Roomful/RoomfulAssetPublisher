using System;
using System.Collections.Generic;
using net.roomful.api.assets;

namespace net.roomful.models
{
    /// <summary>
    /// Data source that supports pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPaginatedDataSource<T> : IDataSource<T> where T : ITemplate
    {
        /// <summary>
        /// Retrieves items.
        /// </summary>
        /// <param name="offset">Item collection start offset.</param>
        /// <param name="size">Amount of items to retrieve.</param>
        /// <param name="callback">Operation complete callback.</param>
        void GetItems(int offset, int size, Action<List<T>> callback);

        /// <summary>
        /// Indicates if data source has all the items cached locally.
        /// </summary>
        bool HasReceivedAllItems { get; }
    }
}
