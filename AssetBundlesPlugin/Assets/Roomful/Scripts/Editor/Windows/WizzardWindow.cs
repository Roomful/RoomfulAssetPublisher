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


		public GUIStyle sectionScrollView = "PreferencesSectionBox";
		public GUIStyle settingsBoxTitle = "OL Title";
		public GUIStyle settingsBox = "OL Box";
		public GUIStyle errorLabel = "WordWrappedLabel";
		public GUIStyle sectionElement = "PreferencesSection";
		public GUIStyle evenRow = "CN EntryBackEven";
		public GUIStyle oddRow = "CN EntryBackOdd";
		public GUIStyle selected = "ServerUpdateChangesetOn";
		public GUIStyle keysElement = "PreferencesKeysElement";
		public GUIStyle warningIcon = "CN EntryWarn";
		public GUIStyle sectionHeader;


		internal class Constants {

			public GUIStyle sectionScrollView = "PreferencesSectionBox";
			public GUIStyle settingsBoxTitle = "OL Title";
			public GUIStyle settingsBox = "OL Box";
			public GUIStyle errorLabel = "WordWrappedLabel";
			public GUIStyle sectionElement = "PreferencesSection";
			public GUIStyle evenRow = "CN EntryBackEven";
			public GUIStyle oddRow = "CN EntryBackOdd";
			public GUIStyle selected = "ServerUpdateChangesetOn";
			public GUIStyle keysElement = "PreferencesKeysElement";
			public GUIStyle warningIcon = "CN EntryWarn";
			public GUIStyle sectionHeader = new GUIStyle(EditorStyles.largeLabel);
			public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);

			public Constants() {
				this.sectionHeader = new GUIStyle(EditorStyles.largeLabel);

				this.sectionScrollView = new GUIStyle(this.sectionScrollView);
				this.sectionScrollView.overflow.bottom++;
				this.sectionHeader.fontStyle = FontStyle.Bold;
				this.sectionHeader.fontSize = 18;
				this.sectionHeader.margin.top = 10;
				this.sectionHeader.margin.left++;

				if (!EditorGUIUtility.isProSkin) {
					this.sectionHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
				} else {
					this.sectionHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
				}

				this.cacheFolderLocation.wordWrap = true;
			}
		}


		private delegate void OnGUIDelegate();

		private class Section {
			public GUIContent content;

			public WizzardWindow.OnGUIDelegate guiFunc;

			public Section(string name, WizzardWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name);
				this.guiFunc = guiFunc;
			}

			public Section(string name, Texture2D icon, WizzardWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name, icon);
				this.guiFunc = guiFunc;
			}

			public Section(GUIContent content, WizzardWindow.OnGUIDelegate guiFunc) {
				this.content = content;
				this.guiFunc = guiFunc;
			}
		}




		private int m_SelectedSectionIndex;
		private Vector2 m_SectionScrollPos;
		private List<WizzardWindow.Section> m_Sections;
		private static WizzardWindow.Constants constants = null;



		//--------------------------------------
		//  Initialisation
		//--------------------------------------


		private void OnEnable() {


			this.m_Sections = new List<WizzardWindow.Section>();
			this.m_Sections.Add(new WizzardWindow.Section("Wizard", new WizzardWindow.OnGUIDelegate(this.Wizard)));
			this.m_Sections.Add(new WizzardWindow.Section("Assets", new WizzardWindow.OnGUIDelegate(this.Assets)));
			this.m_Sections.Add(new WizzardWindow.Section("Platfroms", new WizzardWindow.OnGUIDelegate(this.Platforms)));
			this.m_Sections.Add(new WizzardWindow.Section("Account", new WizzardWindow.OnGUIDelegate(this.Account)));

		}


		//--------------------------------------
		//  GUI Render
		//--------------------------------------

		void OnGUI() {

			GUI.changed = false;
			EditorGUIUtility.labelWidth = 200f;

			if (WizzardWindow.constants == null) {
				WizzardWindow.constants = new WizzardWindow.Constants();
			}



			GUILayout.BeginHorizontal(new GUILayoutOption[0]);

			m_SectionScrollPos = GUILayout.BeginScrollView(this.m_SectionScrollPos, WizzardWindow.constants.sectionScrollView, new GUILayoutOption[]{ GUILayout.Width(120f)});

			GUILayout.Space(40f);
			for (int i = 0; i < this.m_Sections.Count; i++) {
				WizzardWindow.Section section = this.m_Sections[i];

				Rect rect = GUILayoutUtility.GetRect(section.content, WizzardWindow.constants.sectionElement, new GUILayoutOption[]{GUILayout.ExpandWidth(true)});

				if (section == this.selectedSection && Event.current.type == EventType.Repaint) {
					WizzardWindow.constants.selected.Draw(rect, false, false, false, false);
				}

				EditorGUI.BeginChangeCheck();
				if (GUI.Toggle(rect, this.selectedSectionIndex == i, section.content, WizzardWindow.constants.sectionElement)) {
					this.selectedSectionIndex = i;
				} if (EditorGUI.EndChangeCheck()){
					GUIUtility.keyboardControl = 0;
				}
			}


			GUILayout.EndScrollView();
			GUILayout.Space(10f);

			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(this.selectedSection.content, WizzardWindow.constants.sectionHeader, new GUILayoutOption[0]);
			this.selectedSection.guiFunc();
			GUILayout.Space(5f);
			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

			if(GUI.changed) {
				DirtyEditor();
			}
		}



		//--------------------------------------
		//  Get / Set
		//--------------------------------------


		private int selectedSectionIndex {
			get {
				return this.m_SelectedSectionIndex;
			} set {

				this.m_SelectedSectionIndex = value;
				if (this.m_SelectedSectionIndex >= this.m_Sections.Count) {
					this.m_SelectedSectionIndex = 0;
				} else if (this.m_SelectedSectionIndex < 0) {
					this.m_SelectedSectionIndex = this.m_Sections.Count - 1;
				}
			}
		}


		private WizzardWindow.Section selectedSection {
			get {
				return this.m_Sections[this.m_SelectedSectionIndex];
			}
		}


		//--------------------------------------
		//  Tabs
		//--------------------------------------



		//--------------------------------------
		//  Initialization
		//--------------------------------------

		/*

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

*/

		private void Platforms() {
			GUILayout.BeginHorizontal ();

			EditorGUILayout.BeginVertical (GUI.skin.box, GUILayout.Width (200));

			GUILayout.Label ("Current Platform: "+EditorUserBuildSettings.activeBuildTarget.ToString());
			EditorGUILayout.Space ();

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



		private Vector2 m_KeyScrollPos;
		private AssetTemplate SelectedAsset = null;
		private void Assets() {


			if(!AssetBundlesSettings.Instance.LocalAssetTemplates.Contains(SelectedAsset)) {
				SelectedAsset = null;
			}

			if(SelectedAsset == null) {
				if(AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
					SelectedAsset = AssetBundlesSettings.Instance.LocalAssetTemplates [0];
				}
			}


			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();



			GUILayout.BeginVertical( GUILayout.Width(230));

			GUILayout.BeginHorizontal ();
			GUILayout.Label("Avaliable Assets", WizzardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.ExpandWidth(true)});


			Texture2D refreshIcon = Resources.Load ("refresh") as Texture2D;
			bool refresh = GUILayout.Button (refreshIcon, WizzardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if (refresh) {
				AssetRequestManager.RefreshAssetsList ();
			}



			bool addnew = GUILayout.Button ("+", WizzardWindow.constants.settingsBoxTitle, GUILayout.Width (20));
			if(addnew) {
				CreateNewAssetWindow ();
			}



			Texture2D trash = Resources.Load ("trash") as Texture2D;
			bool remove = GUILayout.Button (trash, WizzardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if(remove && SelectedAsset != null) {
				if (EditorUtility.DisplayDialog ("Delete " + SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {;
					AssetRequestManager.RemoveAsset (SelectedAsset);
				}
			}



			GUILayout.EndHorizontal ();

			m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, WizzardWindow.constants.settingsBox,  new GUILayoutOption[] {GUILayout.Width(230)});
			foreach(var asset in AssetBundlesSettings.Instance.LocalAssetTemplates) {

				if (GUILayout.Toggle(SelectedAsset == asset, asset.DisaplyContent, WizzardWindow.constants.keysElement, new GUILayoutOption[0])) {
					SelectedAsset = asset;
				}
			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();





			GUILayout.BeginVertical(GUILayout.Width(230));


			if(SelectedAsset != null) {

				EditorGUILayout.Space ();
				EditorGUILayout.LabelField ("Asset Info", EditorStyles.boldLabel);
				EditorGUILayout.Space ();

				AssetInfoLable ("Id", SelectedAsset.Id);
				AssetInfoLable ("Title", SelectedAsset.Title);
				AssetInfoLable ("Placing", SelectedAsset.Placing);
				AssetInfoLable ("Invoke", SelectedAsset.InvokeType);
				AssetInfoLable ("Can Stack", SelectedAsset.CanStack);
				AssetInfoLable ("Max Scale", SelectedAsset.MaxScale);
				AssetInfoLable ("Min Scale", SelectedAsset.MinScale);

			}


			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

			GUILayout.Space(5f);
			bool restoreDefaults = GUILayout.Button ("Restore Defaults", GUILayout.Width(150));
			if(restoreDefaults) {
				/*	Settings.Tags.Clear (); 
				Settings.InitDefaultTags ();
				LoggerWindow.RefreshUI ();*/
			}

		}


		private void AssetInfoLable(string title, object msg) {
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField (title + ": ",  EditorStyles.boldLabel, new GUILayoutOption[] {GUILayout.Height(15), GUILayout.Width(65)});
			EditorGUILayout.SelectableLabel (msg.ToString(), EditorStyles.label, new GUILayoutOption[] {GUILayout.Height(15)});
			GUILayout.EndHorizontal ();
		}

		private void Assets2() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			GUILayout.BeginHorizontal ();
/*
			if (GUILayout.Button ("Get all assets")) {
				GetAllAssets ();
			}

			if(GUILayout.Button("Create new")) {
				CreateNewAssetWindow ();
			}*/



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




		private void Account() {
			
		}

		private void Wizard() {
			GUILayout.BeginVertical ();

			EditorGUILayout.Space();

			if (EditableProp != null) {

				EditorGUI.BeginChangeCheck ();

				EditableAssetName = EditorGUILayout.TextField ("Name: ", EditableAssetName);

				CurrentPropPlacing = (Placing) EditorGUILayout.EnumPopup("Placing: ", CurrentPropPlacing);
				CurrentPropInvoke = (InvokeTypes) EditorGUILayout.EnumPopup("Invoke Type: ", CurrentPropInvoke);

				CurrentPropThumbnail = (Texture2D) EditorGUILayout.ObjectField("Thumbnail: ", CurrentPropThumbnail, typeof (Texture2D), false);
				EditableProp.Template.CanStack = EditorGUILayout.Toggle ("CanStack", EditableProp.Template.CanStack);

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

				if(GUILayout.Button("Create new")) {
					CreateNewAssetWindow ();
				}
			}

			GUILayout.EndVertical ();
		}

		private void LoadAssetBundle(AssetTemplate prop) {
			EditorApplication.delayCall = () => {
				
				string pl = EditorUserBuildSettings.activeBuildTarget.ToString();

				Network.Request.GetAssetUrl getAssetUrl = new RF.AssetWizzard.Network.Request.GetAssetUrl (prop.Id, pl);

				getAssetUrl.PackageCallbackText = (assetUrl) => {
					Debug.Log(assetUrl);
					Network.Request.GetAsset loadAsset = new RF.AssetWizzard.Network.Request.GetAsset (assetUrl);

					loadAsset.PackageCallbackData = (loadCallback) => {
						string bundlePath = AssetBundlesSettings.AssetBundlesPathFull+"/"+prop.Title+"_"+pl;


						FolderUtils.WriteBytes(bundlePath, loadCallback);

						Caching.CleanCache();

						AssetBundle assetBundle = AssetBundle.LoadFromFile(bundlePath);

						RecreateProp(prop, assetBundle.LoadAsset<Object>(prop.Title));
						assetBundle.Unload(false);
						AssetDatabase.DeleteAsset(bundlePath);
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

			string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + assetName + ".prefab";

			PropAsset createdProp = new GameObject (assetName).AddComponent<PropAsset> ();
			createdProp.Template.Title = assetName;


			FolderUtils.CreateFolder(AssetBundlesSettings.ASSETS_LOCATION);
			GameObject newPrfab = PrefabUtility.CreatePrefab (prefabPath, createdProp.gameObject);
			PrefabUtility.ConnectGameObjectToPrefab (createdProp.gameObject, newPrfab);

			//EditorSceneManager.MarkSceneDirty (EditorSceneManager.GetActiveScene ());
		}

		private void RecreateProp(AssetTemplate tpl, Object prop) {
			if (prop == null) {
				Debug.Log ("Prop is null");
				return;
			}

			string prefabPath = AssetBundlesSettings.FULL_ASSETS_LOCATION + tpl.Title + ".prefab";

			OpenWorkshopScene ();

			GameObject newGo = (GameObject)Instantiate (prop) as GameObject;
			newGo.name = tpl.Title;


			var renderers = newGo.GetComponentsInChildren<Renderer> ();

			foreach (Renderer r in renderers) {
				foreach(Material m in r.sharedMaterials) {
					var shaderName = m.shader.name;
					var newShader = Shader.Find(shaderName);
					if(newShader != null){
						m.shader = newShader;
					} else {
						Debug.LogWarning("unable to refresh shader: "+shaderName+" in material "+m.name);
					}
				}
			}



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
			};

			updateRequest.Send ();
		}

		private void UploadAssets() {

			if(!AssetBundlesManager.ValidateAsset(EditableProp)) {
				return;
			}


			Network.Request.CreateMetaData createMeta = new RF.AssetWizzard.Network.Request.CreateMetaData (EditableProp.Template);

			createMeta.PackageCallbackText = (callback) => { 

				EditableProp.Template.Id =  new AssetTemplate(callback).Id;
				SavePrefab(EditableProp.Template.Title,  EditableProp.gameObject);
				AssetBundlesManager.Clone(EditableProp);

				int counter = 0;

				AssetBundlesManager.AssetsUploadLoop(counter, EditableProp.Template, () => {
					AssetBundlesManager.DelteTempFiles();
					AssetDatabase.Refresh();
					AssetDatabase.SaveAssets();

					EditorApplication.delayCall = () => {
						FolderUtils.DeleteFolder(AssetBundlesSettings.AssetBundlesPath, false);
						FolderUtils.CreateFolder(AssetBundlesSettings.AssetBundlesPath);
					};
				});


				EditorUtility.DisplayDialog ("Success", " Asset has been successfully uploaded!", "Ok");

			};

			createMeta.Send ();
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
			Object prafabObject = AssetDatabase.LoadAssetAtPath(AssetBundlesSettings.FULL_ASSETS_LOCATION+propOnScene.Template.Title+".prefab", typeof(Object));

			PrefabUtility.ReplacePrefab(propOnScene.gameObject, prafabObject, ReplacePrefabOptions.ConnectToPrefab | ReplacePrefabOptions.ReplaceNameBased);

			SavePrefab (propOnScene.Template.Title, propOnScene.gameObject);
		}

		private void SavePrefab(string propName, GameObject propObject) {
			Object prafabObject = AssetDatabase.LoadAssetAtPath(AssetBundlesSettings.FULL_ASSETS_LOCATION+propName+".prefab", typeof(Object));

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



		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private static void DirtyEditor() {

		}

	}
}
