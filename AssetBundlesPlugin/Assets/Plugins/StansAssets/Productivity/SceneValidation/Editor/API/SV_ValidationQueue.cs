using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Productivity.SceneValidator
{
    public class SV_ValidationQueue 
    {

        //200 FPS
        private const double MAX_ALLOWED_TIME_PER_FRAME = 0.005;

        private Queue<GameObject> m_queue = new Queue<GameObject>();
        private SV_ValidationPerfomanceReport m_validationReport;
        private SV_iSceneValidator m_validator;

        private Action m_finishCallback;

        private double m_startTime;
        private double m_timeSlice;


        public SV_ValidationQueue(SV_iSceneValidator validator) {
            m_validator = validator;
            m_validationReport = new SV_ValidationPerfomanceReport(m_validator.Scene);
        }


        public void AddGameObject(GameObject gameObject) {
            m_queue.Enqueue(gameObject);
        }

        public void Clear() {
            m_queue.Clear();
        }

        public void AddGameObject() {

        }


        public void Run(Action callback) {
            m_finishCallback = callback;
            m_startTime = EditorApplication.timeSinceStartup;
            m_timeSlice = m_startTime;
            m_validationReport = new SV_ValidationPerfomanceReport(m_validator.Scene);

            ValidateNext();

        }


        public SV_ValidationPerfomanceReport ValidationReport {
            get {
                return m_validationReport;
            }
        }


        private void ValidateNext() {

            if(m_queue.Count == 0) {
                Finsih();
                return;
            }

            GameObject go = m_queue.Dequeue();
            if(go == null) {
                ValidateNext();
                return;
            }


            m_validator.ValidateGameObject(go, m_validationReport);


            double timeSpent = (EditorApplication.timeSinceStartup - m_timeSlice);


            if (timeSpent > MAX_ALLOWED_TIME_PER_FRAME) {
                SV_Validation.API.HierarchyUI.Repaint();
                SV_SceneInspector.Repaint();
                EditorApplication.delayCall += () => {
                    m_timeSlice = EditorApplication.timeSinceStartup;
                    ValidateNext();
                };
            } else {
                ValidateNext();
            }
        }

        private void Finsih() {

         
            double timeSpent = (EditorApplication.timeSinceStartup - m_startTime);
 
            m_validationReport.TimeSpent = timeSpent;

            m_finishCallback.Invoke();


        }

    }
}