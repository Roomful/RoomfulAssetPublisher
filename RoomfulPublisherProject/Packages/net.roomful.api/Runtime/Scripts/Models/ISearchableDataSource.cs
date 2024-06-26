namespace net.roomful.models
{
    public interface ISearchableDataSource : IClearableDataSource
    {
        string SearchQuery { get; }
        void SetSearchQuery(string query);
    }
}