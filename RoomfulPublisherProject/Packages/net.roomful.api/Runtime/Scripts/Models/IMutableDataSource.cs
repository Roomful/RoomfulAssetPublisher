using net.roomful.api.assets;

namespace net.roomful.models
{
    public interface IMutableDataSource<T> : ISearchableDataSource, IPaginatedDataSource<T> where T : ITemplate
    {
        void DeleteItem(string id);
        void InsertItem(T item);
        void UpdateItem(T item);
    }
}