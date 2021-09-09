using net.roomful.api.sa;

// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api
{
    public interface IDataSource<T> where T : ITemplate
    {
        SA_iSafeEvent<T> OnItemInserted { get; }
        SA_iSafeEvent<T> OnItemUpdated { get; }
        SA_iSafeEvent<T> OnItemRemoved { get; }
        SA_iSafeEvent OnItemsCleared { get; }
    }
}
