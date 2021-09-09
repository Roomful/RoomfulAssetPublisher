using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class ChangeUserPermissionsModel
    {
        [SerializeField]private string m_userId;
        [SerializeField]private bool m_isModerator;
        [SerializeField]private bool m_isPresenter;
        [SerializeField]private bool m_isPromoted;
        [SerializeField]private string m_videoChatId;
        
        public string VideoChatId => m_videoChatId;
        public string UserId => m_userId;
        public bool IsModerator => m_isModerator;
        public bool IsPresenter => m_isPresenter;
        public bool IsPromoted => m_isPromoted;

        public ChangeUserPermissionsModel() { }
        public ChangeUserPermissionsModel(string videoChatId, string userId, bool isModerator, bool isPresenter, bool isPromoted) {
            m_userId = userId;
            m_videoChatId = videoChatId;
            m_isModerator = isModerator;
            m_isPresenter = isPresenter;
            m_isPromoted = isPromoted;
        }
    }
}