namespace net.roomful.api
{
    public interface IUserLocationInfo
    {
        string UserId { get; }
        UserOnlineState State { get; }
        UserLocationPlace Location { get; }
    }
}
