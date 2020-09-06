using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using net.roomful.assets.serialization;

namespace net.roomful.assets.Editor
{
    [CustomEditor(typeof(PropTiledFrame))]
    public class TiledPropFrameEditor : UnityEditor.Editor {


        public override void OnInspectorGUI() {

            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            Frame.Corner =  EditorGUILayout.ObjectField(new GUIContent("Corner"), Frame.Corner, typeof(GameObject), true) as GameObject;
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
            float frameOffset = EditorGUILayout.FloatField("Frame Offset", Frame.Settings.FrameOffset);
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
            float backOffset  = EditorGUILayout.FloatField("Back Offset", Frame.Settings.BackOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Back Offset");
                Frame.Settings.BackOffset = backOffset;
            }

            EditorGUI.BeginChangeCheck();
            Color fillerColor = EditorGUILayout.ColorField("Filler Color", Frame.Settings.FillerColor);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Filler Color");
                Frame.Settings.FillerColor = fillerColor;
            }

            var updateButton = GUILayout.Button("Update");

            if (GUI.changed || updateButton) {
                Frame.UpdateFrame();
            }
           
        }

        private PropTiledFrame Frame {
            get {
                return target as PropTiledFrame;
            }
        }

    }
}