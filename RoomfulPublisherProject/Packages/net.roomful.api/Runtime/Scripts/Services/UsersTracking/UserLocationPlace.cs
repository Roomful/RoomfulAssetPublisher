// Copyright Roomful 2013-2018. All rights reserved.

namespace net.roomful.api
{
    public class UserLocationPlace
    {
        private const string LOCATION_KEY_ROOM = "room";
        private readonly string m_place;
        private readonly string m_placeId;

        public string RoomId { get; } = string.Empty;

        public UserLocationPlace(string place, string placeId) {
            m_place = place;
            m_placeId = placeId;
            if (m_place.Equals(LOCATION_KEY_ROOM)) {
                RoomId = m_placeId;
            }
        }

        public UserLocationPlace(string roomId) {
            m_place = LOCATION_KEY_ROOM;
            m_placeId = roomId;
            RoomId = roomId;
        }

        public bool IsEqualTo(UserLocationPlace location) {
            return location.m_place.Equals(m_place) && location.m_placeId.Equals(m_placeId);
        }
    }
}
