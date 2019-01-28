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
        private Request.BaseWebPackage m_package = null;
        private Action OnComplete = delegate { };

        public EditorWebRequest(UnityWebRequest request, Request.BaseWebPackage package) {
            m_request = request;
            m_package = package;
        }


        public void Send(Action callback) {
            OnComplete += callback;

            #if UNITY_EDITOR
            EditorApplication.update += OnUpdate;
            #endif
            m_request.SendWebRequest();
        }

        private void OnUpdate() {
            m_package.DownloadProgress(m_request.downloadProgress);
            m_package.UploadProgress(m_request.uploadProgress);

            if (m_request.isDone) {
                #if UNITY_EDITOR
                EditorApplication.update -= OnUpdate;
                #endif
                OnComplete();
            }
        }
    }
}