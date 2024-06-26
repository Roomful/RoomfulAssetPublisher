using System.Collections.Generic;
using net.roomful.api.socket;

namespace net.roomful.api
{
    public interface IPropertyData
    {
        string Name { get; }

        Dictionary<string, object> ToRawPayload();
        
    }
    public class UpdatePropertyRequest : ISocketRequest<SocketRequestCallback>
    {
        public IPropertyData Data { get; }
        
        public string Id => "videochat:broadcastAction";

        private readonly string m_roomId;
        private readonly string m_videoChatId;
        private readonly string m_route;

        public UpdatePropertyRequest(string roomId, string videoChatId, string route, IPropertyData data) {
            m_roomId = roomId;
            m_videoChatId = videoChatId;
            m_route = route;
            Data = data;
        }

        public Dictionary<string, object> GenerateData() {
            return new Dictionary<string, object> {
                { "roomId", m_roomId },
                { "videochatId", m_videoChatId },
                { "route", m_route},
                { "dataKey", Data.Name },
                { "data", Data.ToRawPayload() }
            };
        }
    }
}