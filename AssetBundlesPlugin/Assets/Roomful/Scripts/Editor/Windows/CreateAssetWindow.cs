using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public class CreateAssetWindow : EditorWindow {

		public static event System.Action<string> NewAssetCreateClicked = delegate{};
		private string AssetName = "";

		public static CreateAssetWindow InitWindow() {
			CreateAssetWindow window = ScriptableObject.CreateInstance<CreateAssetWindow>();
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
			window.ShowPopup();

			return window;
		}

		void OnGUI() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			GUILayout.Label ("Enter name:");
			AssetName = EditorGUILayout.TextField ("", AssetName);

			if (GUILayout.Button ("Create")) {
				NewAssetCreateClicked (AssetName);

				AssetName = "";
				this.Close ();
			}

			if (GUILayout.Button ("Cancel")) {
				AssetName = "";
				this.Close ();
			}

			GUILayout.EndVertical ();
		}
	}
}
