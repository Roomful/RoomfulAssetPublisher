﻿using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public class CreateAssetWindow : EditorWindow {

		private AssetTemplate Asset = new AssetTemplate();

	
		void OnGUI() {

			Texture2D wizardIcon = Resources.Load ("wizard") as Texture2D;
			GUIContent wizardContent =  new GUIContent(wizardIcon, "");
			EditorGUI.LabelField (new Rect (10, 10, 70, 70), wizardContent);


			GUIContent headerContent = new GUIContent ("Please provide general information required \nfor a new Roomful Asset");
			EditorGUI.LabelField (new Rect (100, 10, 300, 40), headerContent);


			EditorGUI.LabelField (new Rect (100, 50, 300, 16), "Title:");
			Asset.Title = EditorGUI.TextField (new Rect (160, 50, 190, 16), Asset.Title);


			EditorGUI.LabelField (new Rect (100, 70, 300, 16), "Plasing:");
			Asset.Placing = (Placing) EditorGUI.EnumPopup(new Rect (160, 70, 190, 16), Asset.Placing);



			GUILayout.Space(110f);
			GUILayout.BeginHorizontal (); {
				GUILayout.FlexibleSpace ();
				bool cancel = GUILayout.Button ("Cancel", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (cancel) {
					
					Dismiss ();
				}

				bool create = GUILayout.Button ("Create", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (create) {

					AssetBundlesManager.CreateNewAsset (Asset);
					Dismiss ();
				}

				GUILayout.Space (20f);

			}GUILayout.EndHorizontal ();
				
		}

		private void Dismiss() {
			Asset = new AssetTemplate ();
			this.Close ();
		}
	}
}