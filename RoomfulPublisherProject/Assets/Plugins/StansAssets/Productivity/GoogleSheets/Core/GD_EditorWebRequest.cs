using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine.Networking;


namespace SA.Productivity.GoogleSheets
{


    public class GD_EditorWebRequest
    {

        public Action OnComplete = delegate { };
        public Action OnUpdate = delegate { };

        private readonly UnityWebRequest m_request = null;
      

        public GD_EditorWebRequest(string url) {
            m_request = UnityWebRequest.Get(url);
        }

        public GD_EditorWebRequest(UnityWebRequest request) {
            m_request = request;
        }


        public void Send(Action callback) {
            OnComplete += callback;
            #if UNITY_EDITOR
            EditorApplication.update += OnEditorUpdate;
            #endif


            #if UNITY_2017_2_OR_NEWER
            m_request.SendWebRequest();
            #else
            m_request.Send();
            #endif


        }

        public UnityWebRequest Request {
            get {
                return m_request;
            }
        }

        public string DataAsText {
            get {
                return m_request.downloadHandler.text;
            }
        }


        private void OnEditorUpdate() {

            OnUpdate();

            if (m_request.isDone) {
                #if UNITY_EDITOR
                EditorApplication.update -= OnEditorUpdate;
                #endif
                OnComplete();
            }
        }
    }
}