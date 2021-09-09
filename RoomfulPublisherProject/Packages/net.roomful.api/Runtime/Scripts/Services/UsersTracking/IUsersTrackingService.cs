// Copyright Roomful 2013-2018. All rights reserved.

using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    /// <summary>
    /// Provides info about users current location in Roomful.
    /// </summary>
    public interface IUsersTrackingService
    {
        /// <summary>
        /// Event is fired when user location is updated.
        /// </summary>
        event Action<LocationUpdateArgs> OnUserLocationUpdated;

        /// <summary>
        /// Use to check if user is online.
        /// </summary>
        /// <param name="userId">An Id of the user.</param>
        /// <returns>Returns `true` if user is online and `false` otherwise.</returns>
        bool IsUserOnline(string userId);

        /// <summary>
        /// Get users in room list.
        /// Please note that users has to be friends. Otherwise location is not available.
        /// </summary>
        /// <param name="roomId">Id of the room</param>
        /// <returns>List of the users ids in the room.</returns>
        List<string> GetUsersInRoom(string roomId);


        /// <summary>
        /// Retrieves current user location.
        /// </summary>
        /// <param name="userId">Id of the target user.</param>
        /// <returns>Returns user location info or `null` if there is no info about the user location</returns>
        IUserLocationInfo GetUserLocationInfo(string userId);

        /*
        UserLocatorState GetUserState(string userId);
        LocatorModel GetUserLastLocation(string userId);
        List<string> GetFriendsInRoom(string roomId);

        void UpdateFriendInfo(UserLocatorModel userLocation);
        */
    }
}
