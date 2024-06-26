using net.roomful.api.socket;

// Copyright Roomful 2013-2021. All rights reserved.

namespace net.roomful.api
{
    public class VideochatPresenterChangedEvent : ISocketEventListener
    {
        public string Id => "room:videochatPresenterChanged";
        public string UserId { get; private set; } = string.Empty;
        public string VideochatId { get; private set; } = string.Empty;

        public void HandleData(JSONData data) {
            UserId = data.GetValue<string>("userId");
            VideochatId = data.GetValue<string>("videochatId");
        }
    }
}
