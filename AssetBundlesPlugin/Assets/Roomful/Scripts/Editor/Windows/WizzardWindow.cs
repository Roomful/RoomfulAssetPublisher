using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RF.AssetWizzard.Editor {
	public class WizzardWindow : EditorWindow {
		
		private WizzardTabs _CurrentTab = WizzardTabs.All;
		private string[] Tabs = new string[] {"Assets", "Current"};

		//Auth
		private string Mail = string.Empty;
		private string Password = string.Empty;

		//prop creation
		private string EditableAssetName = string.Empty;

		//prop editing
		private PropAsset EditableProp = null;

		private Placing CurrentPropPlacing = Placing.Wall;
		private InvokeTypes CurrentPropInvoke = InvokeTypes.None;
		private Texture2D CurrentPropThumbnail;
		private float MinScale = 0.5f;
		private float MaxScale = 2f;

		//--------------------------------------
		//  Initialization
		//--------------------------------------

		public void Awake() {
			CreateAssetWindow.NewAssetCreateClicked += CreateNewAssetHandler;
		}

		public void OnDestroy() {
			CreateAssetWindow.NewAssetCreateClicked -= CreateNewAssetHandler;
		}

		void OnGUI () {
			EditorGUILayout.Space();

			if (string.IsNullOrEmpty (AssetBundlesSettings.Instance.SessionId)) {
				GUILayout.Label ("Use your Roomful account email and password to sign in.");

				AuthWindow ();
				return;
			}

			GUILayout.BeginVertical();

			GUILayout.BeginHorizontal();
			GUILayout.Label ("Roomful asset wizzard. Logined as: "+AssetBundlesSettings.Instance.SessionId);

			if (GUILayout.Button ("Log Out")) {
				Mail = string.Empty;
				Password = string.Empty;

				AssetBundlesSettings.Instance.SetSessionId(string.Empty);
			}
			GUILayout.EndHorizontal();

			GUILayout.Box("", new GUILayoutOption[]{GUILayout.ExpandWidth(true), GUILayout.Height(1)});

			GUILayout.BeginHorizontal();
			CurrentTab = (WizzardTabs)GUILayout.Toolbar((int)CurrentTab, Tabs);

			GUILayout.EndHorizontal();

			GUILayout.EndVertical ();

			switch (CurrentTab) {
			case WizzardTabs.All:
				
				AllPropsWindow ();
				break;
			case WizzardTabs.Current:
				
				CurrentPropsWindow ();
				break;
			}
		}

		private Vector2 scrollPos;

		private void AllPropsWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Get all assets")) {
				GetAllAssets ();
			}

			if(GUILayout.Button("Create new")) {
				CreateNewAssetWindow ();
			}

			GUILayout.EndHorizontal ();
			EditorGUILayout.Space();

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

			foreach (AssetTemplate prop in AssetBundlesSettings.Instance.LocalAssetTemplates) {
				
				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();

				if (prop.Thumbnail != null) {
					GUILayout.Box (prop.Thumbnail, GUIStyle.none, new GUILayoutOption[]{ GUILayout.Width (18), GUILayout.Height (18) });
				} else {
					GUILayout.Box (new Texture2D(18, 18), GUIStyle.none, new GUILayoutOption[]{ GUILayout.Width (18), GUILayout.Height (18) });
				}

				EditorGUILayout.LabelField ("", prop.Title);
				//prop.IsOpen = EditorGUILayout.Foldout(prop.IsOpen, prop.SceneName);
				
				if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(50))) {
	//				EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
	//				EditorSceneManager.OpenScene(prop.ScenePath);
				}
				bool ItemWasRemoved = false;

				if (GUILayout.Button("Delete", EditorStyles.miniButton, GUILayout.Width(50))) {
					if (EditorUtility.DisplayDialog ("Asset removing", "Are you sure you want to remove this asset?", "Remove", "Cancel")) {
						ItemWasRemoved = true;

						RF.AssetWizzard.Network.Request.RemoveAsset removeRequest = new RF.AssetWizzard.Network.Request.RemoveAsset (prop.Id);

						removeRequest.PackageCallbackData = (removeCallback) => {
							AssetBundlesSettings.Instance.RemoverFromLocalAssetTemplates(prop);
						};

						removeRequest.Send ();
					}
				}

				SA.Common.Editor.Tools.SrotingButtons((object) prop, AssetBundlesSettings.Instance.LocalAssetTemplates);

				if(ItemWasRemoved) {
					return;
				}

				EditorGUILayout.EndHorizontal();
	
	//			if(prop.IsOpen) {
	//
	//				EditorGUILayout.BeginHorizontal();
	//				EditorGUILayout.LabelField("Describtion");
	//				EditorGUILayout.EndHorizontal();
	//
	//				EditorGUILayout.BeginHorizontal();
	//				prop.Description	 = EditorGUILayout.TextArea(prop.Description,  new GUILayoutOption[]{GUILayout.Height(60), GUILayout.Width(200)} );
	//				prop.Icon = (Texture2D) EditorGUILayout.ObjectField("", prop.Icon, typeof (Texture2D), false);
	//				EditorGUILayout.EndHorizontal();
	//
	//			}
	
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndScrollView();

			EditorGUILayout.Space();

			if(GUILayout.Button("Add new")) {
				//RoomEditorMenu.NewProp();
			}

			GUILayout.EndVertical ();
		}

		private void GetAllAssets() {
			RF.AssetWizzard.Network.Request.GetAllAssets allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets ();
			AssetBundlesSettings.Instance.LocalAssetTemplates.Clear ();

			allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
				
				List<object> allAssetsList = SA.Common.Data.Json.Deserialize(allAssetsCallback) as List<object>;

				foreach (object assetData in allAssetsList) {
					AssetTemplate at = new AssetTemplate(new JSONData(assetData).RawData);

					AssetBundlesSettings.Instance.LocalAssetTemplates.Add(at);
				}

			};

			allAssetsRequest.Send ();
		}

		private void CurrentPropsWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			if (EditableProp != null) {
				EditableAssetName = EditorGUILayout.TextField ("Name: ", EditableAssetName);

				CurrentPropPlacing = (Placing) EditorGUILayout.EnumPopup("Placing: ", CurrentPropPlacing);
				CurrentPropInvoke = (InvokeTypes) EditorGUILayout.EnumPopup("Invoke Type: ", CurrentPropInvoke);

				CurrentPropThumbnail = (Texture2D) EditorGUILayout.ObjectField("Thumbnail: ", CurrentPropThumbnail, typeof (Texture2D), false);

				GUILayout.BeginHorizontal ();

				MinScale = EditorGUILayout.FloatField ("Min scale: ",MinScale);
				MaxScale = EditorGUILayout.FloatField ("Max scale: ",MaxScale);

				GUILayout.EndHorizontal ();

				EditorGUILayout.Space();

				GUILayout.BeginHorizontal ();

				if (string.IsNullOrEmpty (EditableProp.Template.Id)) {
					if (GUILayout.Button ("Upload")) {
						UploadNewMetaData (EditableProp);
					}

					if (GUILayout.Button ("Clear")) {
						ClearInputFields ();
					}
				} else {
					if (GUILayout.Button ("Update")) {
						Debug.Log ("Update");
						UploadNewMetaData (EditableProp);
					}

					if (GUILayout.Button ("Reset")) {
						SetInputsByProp ();
					}

					if (GUILayout.Button ("Delete")) {
						Debug.Log ("Delete");
						//LoadAssetBundle ();
					}
				}

				GUILayout.EndHorizontal ();
			} else {
				GUILayout.Label ("Can't find Prop on scene");
			}

			GUILayout.EndVertical ();
		}

		private void LoadAssetBundle() {
			EditorApplication.delayCall = () => {
				EditorApplication.delayCall = null;

				Network.Request.GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl (EditableProp.Template.Id);

				getAssetUrl.PackageCallbackText = (assetUrl) => {

					Network.Request.GetAsset loadAsset = new RF.AssetWizzard.Network.Request.GetAsset (assetUrl);

					loadAsset.PackageCallbackData = (loadCallback) => {
						string bundlePath = AssetBundlesSettings.AssetBundlesPath+"/" + EditableAssetName.ToLower();
						FolderUtils.WriteBytes(bundlePath, loadCallback);

						AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

						RecreateProp(EditableAssetName, assetBundle.LoadAsset<Object>(EditableAssetName.ToLower()));

						//assetBundle.Unload(false);
					};

					loadAsset.Send ();

				};

				getAssetUrl.Send();
			};
		}

		//--------------------------------------
		//  Authorization
		//--------------------------------------

		private void AuthWindow() {
			GUILayout.BeginVertical ();

			Mail = EditorGUILayout.TextField ("Mail: ", Mail);
			Password = EditorGUILayout.TextField ("Password: ", Password);

			if (GUILayout.Button ("Log In")) {
				if (string.IsNullOrEmpty (Mail) || string.IsNullOrEmpty (Password)) {
					Debug.Log ("Fill all inputs ");
				} else {
					Network.Request.Signin signInRequest = new RF.AssetWizzard.Network.Request.Signin (Mail, Password);

					signInRequest.PackageCallbackText = (signInCallback)=> {
						ParseSessionToken (signInCallback);
					};

					signInRequest.Send ();
				}
			}

			GUILayout.EndVertical ();
		}

		private void ParseSessionToken(string resp) {
			Dictionary<string, object> originalJson = SA.Common.Data.Json.Deserialize(resp) as Dictionary<string, object>;
			AssetBundlesSettings.Instance.SetSessionId(originalJson["session_token"].ToString());
		}

		//--------------------------------------
		//  AssetCreation
		//--------------------------------------

		private void CreateNewAssetWindow() {
			CreateAssetWindow.InitWindow ();
		}

		private void CreateNewAssetHandler(string newAsset) {
			if (string.IsNullOrEmpty(newAsset)) {
				Debug.Log ("Prop's name is empty");
				return;
			}

			EditorApplication.delayCall = () => {
				OpenWorkshopScene ();

				CurrentTab = WizzardTabs.Current;

				PropAsset propOnScene = GameObject.FindObjectOfType<PropAsset> ();

				if (propOnScene == null) {
					CreateAsset (newAsset);
				} else {
					Debug.Log("Some prop is already in workshop!");
				}
			};
		}

		private void OpenWorkshopScene() {
			if (EditorSceneManager.GetActiveScene () != EditorSceneManager.GetSceneByPath (AssetBundlesSettings.AssetBundlesWorshopScene)) {
				try {
					EditorSceneManager.OpenScene (AssetBundlesSettings.AssetBundlesWorshopScene);
				} catch {
					CreateWorkshopScene (AssetBundlesSettings.AssetBundlesWorshopScene);
				}
			}
		}

		private void CreateAsset(string assetName) {
			EditableAssetName = assetName;

			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + assetName + ".prefab";

			PropAsset createdProp = new GameObject (assetName).AddComponent<PropAsset> ();
			createdProp.Template.Title = assetName;

			GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, createdProp.gameObject);
			PrefabUtility.ConnectGameObjectToPrefab (createdProp.gameObject, newPrfab);

			//EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
		}

		private void RecreateProp(string propName, Object prop) {
			if (prop == null) {
				Debug.Log ("Prop is null");
				return;
			}

			string scenePath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + propName + "/" + propName + ".unity";
			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + propName + "/" + propName + ".prefab";

			CreateWorkshopScene (scenePath);

			GameObject newGo = (GameObject)Instantiate (prop) as GameObject;
			newGo.name = propName;

//			Prop createdProp = new GameObject (propName).AddComponent<Prop> ();
//			createdProp.Template.Title = propName;

			GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, newGo);
			PrefabUtility.ConnectGameObjectToPrefab (newGo, newPrfab);

			EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
		}

		private void CreateWorkshopScene(string scenePath) {
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

			EditorSceneManager.SaveScene(newScene, scenePath, false);
		}

		private void UploadNewMetaData(PropAsset prop) {
			prop.Template.Title = EditableAssetName;
			prop.Template.Placing = CurrentPropPlacing;
			prop.Template.InvokeType = CurrentPropInvoke;
			prop.Template.Thumbnail = CurrentPropThumbnail;

			prop.Template.MinScale = MinScale;
			prop.Template.MaxScale = MaxScale;

			prop.Template.FileName = EditableAssetName;

			if (prop.Template.Placing == Placing.None) {
				Debug.Log ("Choose placing!");
				return;
			}

			if (prop.Template.InvokeType == InvokeTypes.None) {
				Debug.Log ("Choose invoke type!");
				return;
			}

			if (prop.Template.Thumbnail == null) {
				Debug.Log ("Set asset's thumbnail!");
				//return;
			}

			if (prop.transform.childCount < 1) {
				Debug.Log ("Prop asset is empty!");
				return;
			}

			Network.Request.CreateMetaData createMeta = new RF.AssetWizzard.Network.Request.CreateMetaData (prop.Template);

			createMeta.PackageCallbackText = (callback) => { 
				prop.SetTemplate( new AssetTemplate(callback));

				Repaint();

				Network.Request.GetUploadLink getUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink (prop.Template.Id);

				EditorApplication.delayCall = () => {
					EditorApplication.delayCall = null;

					BuildAssetBundleFor(prop.Template.Title);

					getUploadLink.PackageCallbackText = (linkCallback) => {
						
						byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.AssetBundlesPath+"/"+EditableAssetName.ToLower());

						Network.Request.UploadAsset uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset(linkCallback, assetBytes);

						uploadRequest.PackageCallbackText = (uploadCallback)=> {
							Network.Request.UploadConfirmation confirm = new Network.Request.UploadConfirmation(prop.Template.Id);

							confirm.PackageCallbackText = (confirmCallback)=> {
								CleanAssetBundleName(prop.Template.Title);
								Debug.Log("Prop: "+prop.Template.Title+" uploaded!" );
							};

							confirm.Send();
						};

						uploadRequest.Send();

					};

					getUploadLink.Send();
				};

				Close();
			};

			createMeta.Send ();
		}

		private void BuildAssetBundleFor(string assetName) {
			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + assetName+ ".prefab";

			AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
			assetImporter.assetBundleName = assetName;

			//here should be building for all platforms

			BuildPipeline.BuildAssetBundles (AssetBundlesSettings.AssetBundlesPath, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneOSXUniversal);
		}

		private void CleanAssetBundleName(string assetName) {
			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + assetName+ ".prefab";

			AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
			assetImporter.assetBundleName = string.Empty;

			AssetDatabase.RemoveUnusedAssetBundleNames ();
		}

		private void ClearInputFields() {
			EditableAssetName = string.Empty;
			CurrentPropPlacing = Placing.Wall;
			CurrentPropInvoke = InvokeTypes.None;
			CurrentPropThumbnail = null;
			MinScale = 0.5f;
			MaxScale = 2f;
		}

		private void SetInputsByProp() {
			if (EditableProp == null) {
				EditableProp = GameObject.FindObjectOfType<PropAsset> ();
			}

			if (EditableProp != null) {
				EditableAssetName = EditableProp.Template.Title;

				CurrentPropPlacing = EditableProp.Template.Placing;
				CurrentPropInvoke = EditableProp.Template.InvokeType;

				if (EditableProp.Template.Thumbnail != null) {
					CurrentPropThumbnail = EditableProp.Template.Thumbnail;
				}

				MinScale = EditableProp.Template.MinScale;
				MaxScale = EditableProp.Template.MaxScale;

				CurrentPropThumbnail = EditableProp.Template.Thumbnail;
			}
		}

		public WizzardTabs CurrentTab {
			get {
				return _CurrentTab;
			}
			set {
				if (value == WizzardTabs.Current) {
					if (EditableProp == null) {
						SetInputsByProp ();
					}
				} else {
					EditableProp = null;
				}

				_CurrentTab = value;
			}
		}


	}
}
