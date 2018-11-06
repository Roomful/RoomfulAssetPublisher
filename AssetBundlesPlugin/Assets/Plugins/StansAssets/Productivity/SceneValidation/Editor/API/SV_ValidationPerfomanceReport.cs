using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SA.Productivity.SceneValidator
{
    public class SV_ValidationPerfomanceReport  {

        public int GameobjectsCount = 0;
        public int ComponentsCount = 0;
        public double TimeSpent = -1;

        private Scene m_scene;

        public SV_ValidationPerfomanceReport(Scene scene) {
            m_scene = scene;
        }

        public Scene Scene {
            get {
                return m_scene;
            }
        }

        public double Time3DigitsRound {
            get {
                return Math.Round(TimeSpent, 3);
            }
        }
    }
}