using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.Networking;


namespace RF.AssetWizzard.Network
{


    public class EditorWebRequest
    {

        private UnityWebRequest m_request = null;
        private Action OnComplete = delegate { };

        public EditorWebRequest(UnityWebRequest request) {
            m_request = request;
        }


        public void Send(Action callback) {
            OnComplete += callback;

            #if UNITY_EDITOR
            EditorApplication.update += OnUpdate;
            #endif
            m_request.Send();
        }

        private void OnUpdate() {

            if (m_request.isDone) {
                #if UNITY_EDITOR
                EditorApplication.update -= OnUpdate;
                #endif
                OnComplete();
            }
        }
    }
}