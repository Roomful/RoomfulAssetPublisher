using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class MuteAllModel
    {
        [SerializeField] private string m_videoChatId;
        [SerializeField] private bool m_isMuteAll;

        public string VideoChatId => m_videoChatId;
        public bool IsMuteAll => m_isMuteAll;

        public MuteAllModel() { }
        public MuteAllModel(string videoChatId, bool isMuteAll) {
            m_isMuteAll = isMuteAll;
            m_videoChatId = videoChatId;
        }
    }
}
