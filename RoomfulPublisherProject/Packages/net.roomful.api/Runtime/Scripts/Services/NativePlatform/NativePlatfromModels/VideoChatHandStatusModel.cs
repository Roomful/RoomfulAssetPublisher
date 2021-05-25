using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class VideoChatHandStatusModel
    {
        [SerializeField] private string m_videoChatId;
        [SerializeField] private int m_handStatus;

        public string VideoChatId => m_videoChatId;
        public int HandStatus => m_handStatus;

        public VideoChatHandStatusModel() { }
        public VideoChatHandStatusModel(string videoChatId, int handStatus) {
            m_handStatus = handStatus;
            m_videoChatId = videoChatId;
        }
    }
}