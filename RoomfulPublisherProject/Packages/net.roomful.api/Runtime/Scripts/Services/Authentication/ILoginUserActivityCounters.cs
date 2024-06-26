namespace net.roomful.api.authentication
{
    /// <summary>
    /// User pending activity counters info.
    /// </summary>
    public interface ILoginUserActivityCounters
    {
        /// <summary>
        /// Number of the pending requests.
        /// </summary>
        int PendingRequestCount { get; }

        /// <summary>
        /// Number of the pending contact requests.
        /// </summary>
        int PendingContactsCount { get; }

        /// <summary>
        /// Number of the not yet read notifications by a user.
        /// </summary>
        int NewNotificationsCount { get; }
    }
}
