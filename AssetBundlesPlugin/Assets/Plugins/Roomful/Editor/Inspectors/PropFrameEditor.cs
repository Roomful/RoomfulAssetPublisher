using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor
{
    [CustomEditor(typeof(PropFrame))]
    public class PropFrameEditor : UnityEditor.Editor {


        public override void OnInspectorGUI() {

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Frame", EditorStyles.boldLabel);
            Frame.Corner =  EditorGUILayout.ObjectField(new GUIContent("Corner"), Frame.Corner, typeof(GameObject), true) as GameObject;
            Frame.Border = EditorGUILayout.ObjectField(new GUIContent("Border"), Frame.Border, typeof(GameObject), true) as GameObject;
           


            EditorGUI.BeginChangeCheck();
            float frameOffset = EditorGUILayout.FloatField("Frame Offset", Frame.Settings.FrameOffset);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(Frame.Settings, "Frame Offset");
                Frame.Settings.FrameOffset = frameOffset;
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            Frame.Back = EditorGUILayout.ObjectField(new GUIContent("Back"), Frame.Back, typeof(GameObject), true) as GameObject;


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