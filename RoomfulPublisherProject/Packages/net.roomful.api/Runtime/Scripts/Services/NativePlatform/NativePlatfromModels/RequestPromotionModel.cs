using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class RequestPromotionModel
    {
        [SerializeField] private string m_videoChatId;

        public string VideoChatId => m_videoChatId;

        public RequestPromotionModel() { }
        public RequestPromotionModel(string videoChatId) {
            m_videoChatId = videoChatId;
        }
    }
}
