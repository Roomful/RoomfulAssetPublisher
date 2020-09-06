using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor {
	public class AddPlatformWindow : EditorWindow {

		public static event System.Action<UnityEditor.BuildTarget> NewPlatformAdded = delegate{};
		private BuildTarget _Platform = BuildTarget.NoTarget;

		public static AddPlatformWindow InitWindow() {
			AddPlatformWindow window = ScriptableObject.CreateInstance<AddPlatformWindow>();
			window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
			window.ShowPopup();

			return window;
		}

		void OnGUI() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			GUILayout.Label ("Enter name:");
			_Platform = (BuildTarget) EditorGUILayout.EnumPopup("New Platform: ", _Platform);

			if (GUILayout.Button ("Add")) {
				
				if (_Platform == BuildTarget.NoTarget) {
					Debug.Log ("You must choose platform before adding!");
				} else {
					NewPlatformAdded (_Platform);
				}

				ResetAndCLose ();
			}

			if (GUILayout.Button ("Cancel")) {
				ResetAndCLose ();
			}

			GUILayout.EndVertical ();
		}

		private void ResetAndCLose() {
			_Platform = BuildTarget.NoTarget;
			this.Close ();
		}
	}
}
