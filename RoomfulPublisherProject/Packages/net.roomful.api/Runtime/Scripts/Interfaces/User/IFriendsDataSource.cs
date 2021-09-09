namespace net.roomful.api
{
    public interface IFriendsDataSource : IPaginatedDataSource<IUserFriendTemplate>
    {
        void Clear();
        void SetSearchQuery(string search);
        void InsertItem(IUserFriendTemplate userFriendTemplate);
    }
}
