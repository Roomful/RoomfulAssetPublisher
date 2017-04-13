using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RF.AssetWizzard.Editor {
	public class WizzardWindow : EditorWindow {
		
		private WizzardTabs _CurrentTab = WizzardTabs.All;
		private string[] Tabs = new string[] {"Assets", "Current", "Settings"};

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

		private Vector2 AllPropsScrollPos;

		//--------------------------------------
		//  Initialization
		//--------------------------------------

		public void Awake() {
			CreateAssetWindow.NewAssetCreateClicked += CreateNewAssetHandler;
			AddPlatformWindow.NewPlatformAdded += NewPlatfromAddedHandlre;
		}

		public void OnDestroy() {
			CreateAssetWindow.NewAssetCreateClicked -= CreateNewAssetHandler;
			AddPlatformWindow.NewPlatformAdded -= NewPlatfromAddedHandlre;
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
			case WizzardTabs.Settings:
				SettingsWindow ();

				break;
			}
		}

		private void SettingsWindow() {
			GUILayout.BeginHorizontal ();

			EditorGUILayout.BeginVertical (GUI.skin.box, GUILayout.Width (200));

			EditorGUILayout.LabelField ("Taget Platfroms: ", EditorStyles.boldLabel);

			EditorGUILayout.Space ();

			foreach (var platform in AssetBundlesSettings.Instance.TargetPlatforms) {
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.BeginHorizontal();

				EditorGUILayout.LabelField ("", platform.ToString());

				bool ItemWasRemoved = false;

				if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(18))) {
					if (EditorUtility.DisplayDialog ("Platform removing", "Are you sure you want to remove this platform?", "Remove", "Cancel")) {
						ItemWasRemoved = true;

						AssetBundlesSettings.Instance.TargetPlatforms.Remove (platform);
						AssetBundlesSettings.Save ();
					}
				}

				if(ItemWasRemoved) {
					return;
				}

				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.Space ();

			if (GUILayout.Button ("Add Platform", EditorStyles.miniButton, GUILayout.Width (100))) {
				AddPlatformWindow.InitWindow ();
			}

			EditorGUILayout.EndVertical ();

			GUILayout.EndHorizontal ();
		}

		private void NewPlatfromAddedHandlre(BuildTarget platfrom) {
			if (AssetBundlesSettings.Instance.TargetPlatforms.Contains (platfrom)) {
				return;
			}

			AssetBundlesSettings.Instance.TargetPlatforms.Add (platfrom);

			AssetBundlesSettings.Save ();
		}

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

			AllPropsScrollPos = EditorGUILayout.BeginScrollView(AllPropsScrollPos);

			foreach (AssetTemplate prop in AssetBundlesSettings.Instance.LocalAssetTemplates) {
				
				EditorGUILayout.BeginVertical (GUI.skin.box);
				EditorGUILayout.BeginHorizontal();

				if (prop.Thumbnail != null) {
					GUILayout.Box (prop.Thumbnail, GUIStyle.none, new GUILayoutOption[]{ GUILayout.Width (18), GUILayout.Height (18) });
				} else {
					GUILayout.Box (new Texture2D(18, 18), GUIStyle.none, new GUILayoutOption[]{ GUILayout.Width (18), GUILayout.Height (18) });
				}

				EditorGUILayout.LabelField ("", prop.Title);

				if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(50))) {
					PropAsset propOnScene = GameObject.FindObjectOfType<PropAsset> ();

					if (propOnScene == null) {
						LoadAssetBundle (prop);
					} else {
						if (EditorUtility.DisplayDialog (propOnScene.Template.Title+" in workshop", "There is an asset ("+propOnScene.Template.Title+") in the workshop, if you want to edit, you should save and remove it before. Save and remove?", "Save and remove", "Cancel")) {
							SavePrefab(propOnScene);

							DestroyImmediate(propOnScene.gameObject);

							LoadAssetBundle (prop);
						}
					}
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
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndScrollView();

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

				AssetBundlesSettings.Save();
			};

			allAssetsRequest.Send ();
		}

		private void CurrentPropsWindow() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			if (EditableProp != null) {

				EditorGUI.BeginChangeCheck ();

				EditableAssetName = EditorGUILayout.TextField ("Name: ", EditableAssetName);

				CurrentPropPlacing = (Placing) EditorGUILayout.EnumPopup("Placing: ", CurrentPropPlacing);
				CurrentPropInvoke = (InvokeTypes) EditorGUILayout.EnumPopup("Invoke Type: ", CurrentPropInvoke);

				CurrentPropThumbnail = (Texture2D) EditorGUILayout.ObjectField("Thumbnail: ", CurrentPropThumbnail, typeof (Texture2D), false);

				GUILayout.BeginHorizontal ();

				MinScale = EditorGUILayout.FloatField ("Min scale: ",MinScale);
				MaxScale = EditorGUILayout.FloatField ("Max scale: ",MaxScale);

				GUILayout.EndHorizontal ();

				if (EditorGUI.EndChangeCheck ()) {
					EditableProp.Template.Title = EditableAssetName;
					EditableProp.Template.Placing = CurrentPropPlacing;
					EditableProp.Template.InvokeType = CurrentPropInvoke;
					EditableProp.Template.Thumbnail = CurrentPropThumbnail;

					EditableProp.Template.MinScale = MinScale;
					EditableProp.Template.MaxScale = MaxScale;

					SavePrefab(EditableProp);
				}

				EditorGUILayout.Space();

				GUILayout.BeginHorizontal ();

				if (string.IsNullOrEmpty (EditableProp.Template.Id)) {
					if (GUILayout.Button ("Upload")) {
						UploadAssets ();
					}

					if (GUILayout.Button ("Clear")) {
						ClearInputFields ();
					}
				} else {
					if (GUILayout.Button ("Update")) {
						UpdateAsset ();
					}

					if (GUILayout.Button ("Reset")) {
						SetInputsByProp ();
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

		private void LoadAssetBundle(AssetTemplate prop) {
			EditorApplication.delayCall = () => {
				EditorApplication.delayCall = null;

//				Network.Request.GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl (prop.Id);
//
//				getAssetUrl.PackageCallbackText = (assetUrl) => {
//
//					Network.Request.GetAsset loadAsset = new RF.AssetWizzard.Network.Request.GetAsset (assetUrl);
//
//					loadAsset.PackageCallbackData = (loadCallback) => {
//						string bundlePath = AssetBundlesSettings.AssetBundlesPath+"/" + prop.Title.ToLower();
//
//						FolderUtils.WriteBytes(bundlePath, loadCallback);
//
//						Caching.CleanCache();
//
//						AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);
//
//						RecreateProp(prop, assetBundle.LoadAsset<Object>(prop.Title.ToLower()));
//
//						assetBundle.Unload(false);
//					};
//
//					loadAsset.Send ();
//
//				};
//
//				getAssetUrl.Send();
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
					if (EditorUtility.DisplayDialog (propOnScene.Template.Title+" in workshop", "There is an asset ("+propOnScene.Template.Title+") in the workshop, if you want to create new, you should save and remove it before. Save and remove?", "Save and remove", "Cancel")) {
						SavePrefab(propOnScene);

						DestroyImmediate(propOnScene.gameObject);

						CreateAsset (newAsset);
					}
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

		private void RecreateProp(AssetTemplate tpl, Object prop) {
			if (prop == null) {
				Debug.Log ("Prop is null");
				return;
			}

			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + tpl.Title + ".prefab";

			OpenWorkshopScene ();

			GameObject newGo = (GameObject)Instantiate (prop) as GameObject;
			newGo.name = tpl.Title;

			newGo.AddComponent<PropAsset> ().SetTemplate (tpl);

			GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, newGo);
			PrefabUtility.ConnectGameObjectToPrefab (newGo, newPrfab);

			CurrentTab = WizzardTabs.Current;
		}

		private void CreateWorkshopScene(string scenePath) {
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
			var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

			EditorSceneManager.SaveScene(newScene, scenePath, false);
		}

		private void UpdateAsset() {
			if (EditableProp.Template.Placing == Placing.None) {
				Debug.Log ("Choose placing!");
				return;
			}

			if (EditableProp.Template.Thumbnail == null) {
				Debug.Log ("Set asset's thumbnail!");
				return;
			}

			if (EditableProp.transform.childCount < 1) {
				Debug.Log ("Prop asset is empty!");
				return;
			}

			RF.AssetWizzard.Network.Request.UpdateAsset updateRequest = new RF.AssetWizzard.Network.Request.UpdateAsset (EditableProp.Template);

			updateRequest.PackageCallbackText = (updateCalback) => {
				EditableProp.SetTemplate(new AssetTemplate(updateCalback));
				Debug.Log(EditableProp.Template.Placing);
			};

			updateRequest.Send ();
		}

		private void UploadAssets() {
			if (EditableProp.Template.Placing == Placing.None) {
				Debug.Log ("Choose placing!");
				return;
			}

			if (EditableProp.Template.Thumbnail == null) {
				Debug.Log ("Set asset's thumbnail!");
				return;
			}

			if (EditableProp.transform.childCount < 1) {
				Debug.Log ("Prop asset is empty!");
				return;
			}

			Network.Request.CreateMetaData createMeta = new RF.AssetWizzard.Network.Request.CreateMetaData (EditableProp.Template);

			createMeta.PackageCallbackText = (callback) => { 
				GameObject CachedPropObject = EditableProp.gameObject;
				AssetTemplate CachedPropTempolate = new AssetTemplate(callback);


				DestroyImmediate(EditableProp);
				EditableProp = null;

				SavePrefab(CachedPropTempolate.Title, CachedPropObject);

				int counter = 0;
				AssetsUploadLoop(counter, CachedPropTempolate, () => {
					
					CleanAssetBundleName(CachedPropTempolate.Title);
					EditableProp = CachedPropObject.AddComponent<PropAsset>();
					EditableProp.SetTemplate(CachedPropTempolate);

					SavePrefab(EditableProp);
				});


				Close();
			};

			createMeta.Send ();
		}

		private void AssetsUploadLoop(int i, AssetTemplate tpl, System.Action FinishHandler) {
			
			if (i < AssetBundlesSettings.Instance.TargetPlatforms.Count) {
				BuildTarget pl = AssetBundlesSettings.Instance.TargetPlatforms [i];	

				Network.Request.GetUploadLink getUploadLink = new RF.AssetWizzard.Network.Request.GetUploadLink (tpl.Id, pl, tpl.Title);

				EditorApplication.delayCall = () => {
					EditorApplication.delayCall = null;

					BuildAssetBundleFor(tpl.Title, pl);

					getUploadLink.PackageCallbackText = (linkCallback) => {

						byte[] assetBytes = System.IO.File.ReadAllBytes(AssetBundlesSettings.AssetBundlesPath+"/"+tpl.Title+"."+pl.ToString());

						Network.Request.UploadAsset uploadRequest = new RF.AssetWizzard.Network.Request.UploadAsset(linkCallback, assetBytes);

						uploadRequest.PackageCallbackText = (uploadCallback)=> {
							Network.Request.UploadConfirmation confirm = new Network.Request.UploadConfirmation(tpl.Id, pl);

							confirm.PackageCallbackText = (confirmCallback)=> {
								i++;

								if (i == AssetBundlesSettings.Instance.TargetPlatforms.Count) {
									FinishHandler();
								} else {
									AssetsUploadLoop(i, tpl, FinishHandler);
								}

							};

							confirm.Send();
						};

						uploadRequest.Send();

					};

					getUploadLink.Send();
				};
			}
		}

		private void BuildAssetBundleFor(string assetName, BuildTarget platform) {
			string prefabPath = AssetBundlesSettings.PROPS_ASSETS_LOCATION + assetName+ ".prefab";

			AssetImporter assetImporter = AssetImporter.GetAtPath (prefabPath);
			assetImporter.assetBundleName = assetName;

			BuildPipeline.BuildAssetBundles (AssetBundlesSettings.AssetBundlesPath, BuildAssetBundleOptions.UncompressedAssetBundle, platform);

			AssetDatabase.RenameAsset (AssetBundlesSettings.AssetBundlesPath+"/"+assetName.ToLower(), assetName+"."+platform.ToString());
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

		private void SavePrefab(PropAsset propOnScene) {
			Object prafabObject = AssetDatabase.LoadAssetAtPath(AssetBundlesSettings.PROPS_ASSETS_LOCATION+propOnScene.Template.Title+".prefab", typeof(Object));

			PrefabUtility.ReplacePrefab(propOnScene.gameObject, prafabObject, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);

			SavePrefab (propOnScene.Template.Title, propOnScene.gameObject);
		}

		private void SavePrefab(string propName, GameObject propObject) {
			Object prafabObject = AssetDatabase.LoadAssetAtPath(AssetBundlesSettings.PROPS_ASSETS_LOCATION+propName+".prefab", typeof(Object));

			PrefabUtility.ReplacePrefab(propObject, prafabObject, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);
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
