using System;
using UnityEngine;

namespace net.roomful.api.native
{
    [Serializable]
    public class NativeContentPickedModel
    {
        [SerializeField] private string m_url;

        public string Url => m_url;

        public NativeContentPickedModel() { }
        public NativeContentPickedModel(string url) {
            m_url = url;
        }
    }
}
