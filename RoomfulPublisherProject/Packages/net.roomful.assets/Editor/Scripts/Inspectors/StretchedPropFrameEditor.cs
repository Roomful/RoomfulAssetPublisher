﻿using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [CustomEditor(typeof(PropStretchedFrame))]
    internal class StretchedPropFrameEditor : Editor
    {
        public override void OnInspectorGUI() {
            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            Frame.Corner = EditorGUILayout.ObjectField(new GUIContent("Corner"), Frame.Corner, typeof(GameObject), true) as GameObject;
            if (EditorGUI.EndChangeCheck()) {
                if (Frame.Corner.GetComponent<Collider>() == null) {
                    Frame.Corner.AddComponent<BoxCollider>();
                }
            }

            EditorGUI.BeginChangeCheck();
            Frame.Border = EditorGUILayout.ObjectField(new GUIContent("Border"), Frame.Border, typeof(GameObject), true) as GameObject;
            if (EditorGUI.EndChangeCheck()) {
                if (Frame.Border.GetComponent<Collider>() == null) {
                    Frame.Border.AddComponent<BoxCollider>();
                }
            }

            EditorGUI.BeginChangeCheck();
            var frameOffset = EditorGUILayout.FloatField("Frame Offset", Frame.Settings.FrameOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Frame Offset");
                Frame.Settings.FrameOffset = frameOffset;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            Frame.Back = EditorGUILayout.ObjectField(new GUIContent("Back"), Frame.Back, typeof(GameObject), true) as GameObject;
            if (EditorGUI.EndChangeCheck()) {
                if (Frame.Back.GetComponent<Collider>() == null) {
                    Frame.Back.AddComponent<BoxCollider>();
                }
            }

            EditorGUI.BeginChangeCheck();
            var backOffset = EditorGUILayout.FloatField("Back Offset", Frame.Settings.BackOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Back Offset");
                Frame.Settings.BackOffset = backOffset;
            }

            var updateButton = GUILayout.Button("Update");

            if (GUI.changed || updateButton) {
                Frame.UpdateFrame();
            }
        }

        private PropStretchedFrame Frame => target as PropStretchedFrame;
    }
}
