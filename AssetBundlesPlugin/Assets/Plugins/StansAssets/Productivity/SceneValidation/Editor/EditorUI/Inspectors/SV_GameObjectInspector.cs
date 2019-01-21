using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;


namespace SA.Productivity.SceneValidator
{
    [CustomEditor(typeof(GameObject))]
    [CanEditMultipleObjects]
    public class SV_GameObjectInspector : Editor
    {
        //Unity's built-in editor
        Editor defaultEditor;


        void OnEnable() {
            //When this inspector is created, also create the built-in inspector
            defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.GameObjectInspector, UnityEditor"));
        }

        void OnDisable() {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable


#if UNITY_2018_3_OR_NEWER
            //Hope this is only for beta, since if we do not destory it here
            // this is a potential memeory leak.
#else
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null) {
                disableMethod.Invoke(defaultEditor, null);
            }      
#endif
            DestroyImmediate(defaultEditor);
        }

        protected override void OnHeaderGUI() {
            DrawDefaultHeader();
        }

        public override void OnInspectorGUI() {

            DrawDefaultInspectorGUI();
            if (targets.Length > 1) {
                return;
            }

            SV_ValidationInspectorUI.DrawValidationUI(target as GameObject);

        }

        

        public void DrawDefaultHeader() {
            defaultEditor.DrawHeader();
        }


        public void DrawDefaultInspectorGUI() {
            defaultEditor.OnInspectorGUI();
        }
   

        

      
    }

}