using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class ConnectedToVideoChatModel
    {
        [SerializeField] private string m_videoChatId;
        [SerializeField] private string m_identity;

        public string VideoChatId => m_videoChatId;
        public string Identity => m_identity;

        public ConnectedToVideoChatModel() { }
        public ConnectedToVideoChatModel(string videoChatId, string identity) {
            m_videoChatId = videoChatId;
            m_identity = identity;
        }
    }
}
