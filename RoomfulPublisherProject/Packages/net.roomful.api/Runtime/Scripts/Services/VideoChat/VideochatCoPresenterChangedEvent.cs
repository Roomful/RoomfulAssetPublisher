using net.roomful.api.socket;

namespace net.roomful.api
{
    public class VideochatCoPresenterChangedEvent : ISocketEventListener
    {
        public string Id => "room:videochatCoPresenterChanged";
        public string UserId { get; private set; } = string.Empty;
        public string VideochatId { get; private set; } = string.Empty;

        public void HandleData(JSONData data) {
            UserId = data.GetValue<string>("userId");
            VideochatId = data.GetValue<string>("videochatId");
        }
    }
}