using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class MicStatusModel
    {
        [SerializeField] private int m_microphoneStatus;
        [SerializeField] private string m_videoChatId;

        public string VideoChatId => m_videoChatId;
        public int MicrophoneStatus => m_microphoneStatus;

        public MicStatusModel() { }
        public MicStatusModel(string videoChatId, int microphoneStatus) {
            m_videoChatId = videoChatId;
            m_microphoneStatus = microphoneStatus;
        }
    }
}
