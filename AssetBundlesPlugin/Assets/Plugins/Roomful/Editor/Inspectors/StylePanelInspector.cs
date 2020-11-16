﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



namespace RF.AssetWizzard.Editor {

	[CustomEditor(typeof(StylePanel))]
	public class StylePanelInspector : UnityEditor.Editor
    {



		void OnEnable() {

		}


		public override void OnInspectorGUI() {

			serializedObject.Update();


            if(Panel.IsFirstPanel) {
                EditorGUILayout.HelpBox("This is your default Start panel, no Icon needed", MessageType.Info);
            } else {
                if(Panel.IsLastPanel) {
                    EditorGUILayout.HelpBox("This is your default End panel, no Icon needed", MessageType.Info);
                } else {
                    Panel.Icon = (Texture2D)EditorGUILayout.ObjectField("Icon:", Panel.Icon, typeof(Texture2D), true);
                }
            }


            Vector3 def = Panel.Bounds.size * 100;
            EditorGUILayout.LabelField("Size(cm): " + (int)def.x + "x" + (int)def.y + "x" + (int)def.z);

            serializedObject.ApplyModifiedProperties ();

		}



        private StylePanel Panel {
            get {
                return target as StylePanel;
            }
        }

	
	}
}