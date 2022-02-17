using System;
using net.roomful.models;

// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api.activity
{
    /// <summary>
    /// Notifications list paginated data source.
    /// </summary>
    public interface INotificationList : IPaginatedDataSource<IActivityNotification>
    {
        /// <summary>
        /// Event fired when notification counter is changed.
        /// </summary>
        event Action OnNewNotificationCounterChanged;

        /// <summary>
        /// Gives number of the new (not yet seen byt a user) notifications.
        /// </summary>
        int NewNotifications { get; }
    }
}
