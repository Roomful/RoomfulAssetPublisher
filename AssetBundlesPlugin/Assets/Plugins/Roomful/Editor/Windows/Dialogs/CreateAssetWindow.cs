using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public class CreateAssetWindow : EditorWindow {

		private PropTemplate m_template = new PropTemplate();

	
		void OnGUI() {

            Texture2D wizardIcon = IconManager.GetIcon(Icon.wizard); 
			GUIContent wizardContent =  new GUIContent(wizardIcon, "");
			EditorGUI.LabelField (new Rect (10, 10, 70, 70), wizardContent);


			GUIContent headerContent = new GUIContent ("Please provide general information required \nfor a new Roomful Asset");
			EditorGUI.LabelField (new Rect (100, 10, 300, 40), headerContent);


			EditorGUI.LabelField (new Rect (100, 50, 300, 16), "Title:");
			m_template.Title = EditorGUI.TextField (new Rect (160, 50, 190, 16), m_template.Title);


			EditorGUI.LabelField (new Rect (100, 70, 300, 16), "Plasing:");
			m_template.Placing = (Placing) EditorGUI.EnumPopup(new Rect (160, 70, 190, 16), m_template.Placing);



			GUILayout.Space(110f);
			GUILayout.BeginHorizontal (); {
				GUILayout.FlexibleSpace ();
				bool cancel = GUILayout.Button ("Cancel", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (cancel) {
					
					Dismiss ();
				}

				bool create = GUILayout.Button ("Create", EditorStyles.miniButton, new GUILayoutOption[]{ GUILayout.Width(80)});
				if (create) {

                    BundleService.Create<PropTemplate>(m_template);
					Dismiss ();
				}

				GUILayout.Space (20f);

			}GUILayout.EndHorizontal ();
				
		}

		private void Dismiss() {
			m_template = new PropTemplate ();
			this.Close ();
		}
	}
}
