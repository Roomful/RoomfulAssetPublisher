namespace net.roomful.api.authentication
{
    public interface ILoginUserActivityCounters
    {
        int PendingRequestCount { get; }

        int PendingContactsCount { get; }

        int NewNotificationsCount { get; }
    }
}
