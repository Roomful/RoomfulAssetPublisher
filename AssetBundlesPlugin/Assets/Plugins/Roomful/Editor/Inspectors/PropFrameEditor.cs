using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor
{
    [CustomEditor(typeof(PropFrame))]
    public class PropFrameEditor : UnityEditor.Editor {


        public GameObject Corner;
        public GameObject Border;
        public GameObject Back;

        SerializedProperty m_corner;
        SerializedProperty m_border;
        SerializedProperty m_back;

        void OnEnable() {
            m_corner = serializedObject.FindProperty("Corner");
            m_border = serializedObject.FindProperty("Border");
            m_back = serializedObject.FindProperty("Back");
        }

        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_corner);
            EditorGUILayout.PropertyField(m_border);
           


            EditorGUI.BeginChangeCheck();
            float frameOffset = EditorGUILayout.FloatField("Frame Offset", Frame.Settings.FrameOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Frame Offset");
                Frame.Settings.FrameOffset = frameOffset;
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_back); 
           

            EditorGUI.BeginChangeCheck();
            float backOffset  = EditorGUILayout.FloatField("Back Offset", Frame.Settings.BackOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Back Offset");
                Frame.Settings.BackOffset = backOffset;
            }



            if (EditorGUI.EndChangeCheck()) {
                Frame.Update();
            }
           
        }


        public PropFrame Frame {
            get {
                return target as PropFrame;
            }
        }

    }
}