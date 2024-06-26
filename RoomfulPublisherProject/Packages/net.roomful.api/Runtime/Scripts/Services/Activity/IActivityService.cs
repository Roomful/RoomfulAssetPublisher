namespace net.roomful.api.activity
{
    /// <summary>
    /// Service gives ability to work with activity.
    /// </summary>
    public interface IActivityService
    {

        /// <summary>
        /// Current activity notifications list.
        /// </summary>
        INotificationList NotificationList { get; }

        /// <summary>
        /// Clear notifications counter.
        /// </summary>
        void ClearNewNotificationsCounter();

        /// <summary>
        /// Delete activity notification by id.
        /// </summary>
        /// <param name="id">Notification id.</param>
        void DeleteNotification(string id);

        /// <summary>
        /// Register notification template parser.
        /// </summary>
        /// <param name="parser">The notification parser instance.</param>
        void RegisterNotificationParser(INotificationParser parser);
    }

    /// <summary>
    /// Notifications parser to parse custom notification template.
    /// </summary>
    public interface INotificationParser
    {
        /// <summary>
        /// Type of the notification to parse.
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Instantiate notification template from the JSON data.
        /// </summary>
        /// <param name="data">Json data that contains notification info.</param>
        /// <returns></returns>
        IActivityNotification Parse(JSONData data);
    }
}
