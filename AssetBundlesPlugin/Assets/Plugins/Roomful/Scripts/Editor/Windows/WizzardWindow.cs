using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RF.AssetWizzard {
	public class WizzardWindow : EditorWindow {
		
		private WizzardTabs CurrentTab = WizzardTabs.All;
		private string[] Tabs = new string[] {"Show all", "Edit current", "Create new"};

		//prop creation
		private string NewPropName = string.Empty;
		private Placing CurrentPropPlacing = Placing.Wall;
		private InvokeTypes CurrentPropInvoke = InvokeTypes.None;
		private Texture2D CurrentPropThumbnail;
		private float MinScale = 0.5f;
		private float MaxScale = 2f;

		void OnGUI () {
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();

			CurrentTab = (WizzardTabs)GUILayout.Toolbar((int)CurrentTab, Tabs);

			GUILayout.EndHorizontal();

			switch (CurrentTab) {
			case WizzardTabs.All:
				
				AllPropsWindow ();
				break;
			case WizzardTabs.Current:
				
				CurrentPropsWindow ();
				break;
			case WizzardTabs.Create:
				
				CreateNewWindow ();
				break;
			}
		}

		private void AllPropsWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();
			GUILayout.Label ("All existing props");

			GUILayout.EndVertical ();
		}

		private void CurrentPropsWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			Prop editableProp = GameObject.FindObjectOfType<Prop> ();

			if (editableProp != null) {
				GUILayout.Label ("Prop: "+NewPropName);

				NewPropName = EditorGUILayout.TextField ("Name: ", NewPropName);

				CurrentPropPlacing = (Placing) EditorGUILayout.EnumPopup("Placing: ", CurrentPropPlacing);
				CurrentPropInvoke = (InvokeTypes) EditorGUILayout.EnumPopup("Invoke Type: ", CurrentPropInvoke);

				CurrentPropThumbnail = (Texture2D) EditorGUILayout.ObjectField("Thumbnail: ", CurrentPropThumbnail, typeof (Texture2D), false);

				GUILayout.BeginHorizontal ();

				MinScale = EditorGUILayout.FloatField ("Min scale: ",MinScale);
				MaxScale = EditorGUILayout.FloatField ("Max scale: ",MaxScale);

				GUILayout.EndHorizontal ();

				EditorGUILayout.Space();

				GUILayout.BeginHorizontal ();

				if (string.IsNullOrEmpty (editableProp.Template.Id)) {
					if (GUILayout.Button ("Upload")) {
						UploadNewMetaData (editableProp);
					}

					if (GUILayout.Button ("Clear")) {
						ClearEditFields ();
					}
				} else {
					if (GUILayout.Button ("Update")) {
						Debug.Log ("Update");
					}

					if (GUILayout.Button ("Reset")) {
						Debug.Log ("Reset");
					}

					if (GUILayout.Button ("Delete")) {
						Debug.Log ("Delete");
					}
				}

				GUILayout.EndHorizontal ();


			} else {
				GUILayout.Label ("Can't find Prop on scene");
			}

			GUILayout.EndVertical ();
		}

		private void CreateNewWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			GUILayout.Label ("Enter name:");
			NewPropName = EditorGUILayout.TextField ("", NewPropName);

			if (GUILayout.Button ("Create")) {
				CreateProp ();

				CurrentTab = WizzardTabs.Current;
			}

			GUILayout.EndVertical ();
		}

		private void CreateProp() {
			if (string.IsNullOrEmpty(NewPropName)) {
				Debug.Log ("Prop's name is empty");
				return;
			}

			EditorUtils.CreateFolder(AssetBundlesSettings.PROPS_LOCATION+NewPropName);

			string scenePath = "Assets/" + AssetBundlesSettings.PROPS_LOCATION + NewPropName + "/" + NewPropName + ".unity";

			CreatePropScene (scenePath, NewPropName);

			Prop createdProp = new GameObject (NewPropName).AddComponent<Prop> ();
			createdProp.Template.AssetName = NewPropName;
		}

		private void CreatePropScene(string scenePath, string propName) {
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

			EditorSceneManager.SaveScene(newScene, scenePath, false);
		}

		private void UploadNewMetaData(Prop prop) {
			prop.Template.AssetName = NewPropName;
			prop.Template.Placing = CurrentPropPlacing.ToString ();
			prop.Template.InvokeType = CurrentPropInvoke.ToString ();

			if (CurrentPropThumbnail != null) {
				byte[] bytes = CurrentPropThumbnail.EncodeToPNG();

				prop.Template.Thumbnail = System.Convert.ToBase64String (bytes);
			}

			prop.Template.MinScale = MinScale;
			prop.Template.MaxScale = MaxScale;
		}

		private void ClearEditFields() {
			NewPropName = string.Empty;
			CurrentPropPlacing = Placing.Wall;
			CurrentPropInvoke = InvokeTypes.None;
			CurrentPropThumbnail = null;
			MinScale = 0.5f;
			MaxScale = 2f;
		}
	}
}
