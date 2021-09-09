using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public interface IPaginatedDataSource<T> : IDataSource<T> where T : ITemplate
    {
        void GetItems(int offset, int size, Action<List<T>> callback);
        bool HasReceivedAllItems { get; }
    }
}
