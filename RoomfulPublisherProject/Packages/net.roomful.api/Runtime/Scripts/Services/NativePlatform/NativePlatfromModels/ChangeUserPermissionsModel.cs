using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class ChangeUserPermissionsModel
    {
        [SerializeField]private string m_userId;
        [SerializeField]private bool m_isModerator;
        [SerializeField]private bool m_isOwner;
        [SerializeField]private bool m_isPanelist;
        [SerializeField]private bool m_isPresenter;
        [SerializeField]private bool m_isCoPresenter;
        [SerializeField]private bool m_isPromoted;
        [SerializeField]private bool m_isParticipant;
        [SerializeField]private bool m_isListener;
        [SerializeField]private string m_videoChatId;
        
        public string VideoChatId => m_videoChatId;
        public string UserId => m_userId;
        public bool IsModerator => m_isModerator;
        public bool IsOwner => m_isOwner;
        public bool IsPanelist => m_isPanelist;
        public bool IsPresenter => m_isPresenter;
        public bool IsCoPresenter => m_isCoPresenter;
        public bool IsPromoted => m_isPromoted;
        public bool IsParticipant => m_isParticipant;
        public bool IsListener => m_isListener;

        public ChangeUserPermissionsModel() { }
        public ChangeUserPermissionsModel(string videoChatId, string userId, bool isModerator, bool isOwner, bool isPanelist, bool isPresenter, bool isCoPresenter, bool isPromoted, bool isParticipant, bool isListener) {
            m_userId = userId;
            m_videoChatId = videoChatId;
            m_isModerator = isModerator;
            m_isOwner = isOwner;
            m_isPanelist = isPanelist;
            m_isPresenter = isPresenter;
            m_isCoPresenter = isCoPresenter;
            m_isPromoted = isPromoted;
            m_isParticipant = isParticipant;
            m_isListener = isListener;
        }
    }
}