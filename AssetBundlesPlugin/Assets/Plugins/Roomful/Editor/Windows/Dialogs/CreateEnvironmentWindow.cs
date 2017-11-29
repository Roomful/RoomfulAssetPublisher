﻿using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public class CreateEnvironmentWindow : EditorWindow {

		private EnvironmentTemplate m_template = new EnvironmentTemplate();

	
		void OnGUI() {

            Texture2D wizardIcon = IconManager.GetIcon(Icon.wizard); 
			GUIContent wizardContent =  new GUIContent(wizardIcon, "");
			EditorGUI.LabelField (new Rect (10, 10, 70, 70), wizardContent);


			GUIContent headerContent = new GUIContent ("Please provide general information about \nyour new Roomful Environment");
			EditorGUI.LabelField (new Rect (100, 10, 300, 40), headerContent);


			EditorGUI.LabelField (new Rect (100, 50, 300, 16), "Name:");
			m_template.Title = EditorGUI.TextField (new Rect (160, 50, 190, 16), m_template.Title);



			GUILayout.Space(110f);
			GUILayout.BeginHorizontal (); {
				GUILayout.FlexibleSpace ();
				bool cancel = GUILayout.Button ("Cancel", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (cancel) {
					Dismiss ();
				}

				bool create = GUILayout.Button ("Create", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (create) {
                    BundleService.Create<EnvironmentTemplate>(m_template);
					Dismiss ();
				}

				GUILayout.Space (20f);

			}GUILayout.EndHorizontal ();
				
		}

		private void Dismiss() {
			m_template = new EnvironmentTemplate();
			this.Close ();
		}
	}
}
