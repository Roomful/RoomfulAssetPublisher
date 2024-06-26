using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class DispatchedDeepLinkModel
    {
        [SerializeField] private string m_payload;

        public string Payload => m_payload;

        public DispatchedDeepLinkModel() { }

        public DispatchedDeepLinkModel(string payload) {
            m_payload = payload;
        }
    }
}
