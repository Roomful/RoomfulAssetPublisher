using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor {
	public class WizzardWindow : EditorWindow {

		//Auth
		private string Mail = string.Empty;
		private string Password = string.Empty;


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
			this.m_Sections.Add(new WizzardWindow.Section("Wizzard", new WizzardWindow.OnGUIDelegate(this.Wizzard)));
			this.m_Sections.Add(new WizzardWindow.Section("Assets", new WizzardWindow.OnGUIDelegate(this.Assets)));
			this.m_Sections.Add(new WizzardWindow.Section("Settings", new WizzardWindow.OnGUIDelegate(this.Settings)));
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

		private PropAsset CurrentProp {
			get {
				return GameObject.FindObjectOfType<PropAsset> ();
			}
		}


		//--------------------------------------
		//  Public Methods
		//--------------------------------------

		public void SiwtchTab(WizzardTabs tab) {
			selectedSectionIndex = (int)tab;
		}




		//--------------------------------------
		//  Wizzard
		//--------------------------------------


		private void Wizzard() {

			GUILayout.Space(10f);
			if (CurrentProp == null) {
				GUILayout.Label ("Can't find Prop on scene");
				if(GUILayout.Button("Create new")) {
					WindowManager.ShowCreateNewAsset ();
				}
				return;
			}

			EditorGUI.BeginChangeCheck ();	{


				GUILayout.BeginHorizontal(); 

				GUILayout.BeginVertical( GUILayout.Width(370)); {

					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Title: ", GUILayout.Width (100));
					GUI.enabled = false;
					CurrentProp.Template.Title = EditorGUILayout.TextField (CurrentProp.Template.Title, GUILayout.Width (240));
					GUI.enabled = true;
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Placing: ", GUILayout.Width (100));
					CurrentProp.Template.Placing = (Placing) EditorGUILayout.EnumPopup(CurrentProp.Template.Placing, GUILayout.Width (240));
					GUILayout.EndHorizontal ();


					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Invoke Type: ", GUILayout.Width (100));
					CurrentProp.Template.InvokeType = (InvokeTypes) EditorGUILayout.EnumPopup(CurrentProp.Template.InvokeType, GUILayout.Width (240));
					GUILayout.EndHorizontal ();

					CurrentProp.Template.CanStack = YesNoFiled ("CanStack", CurrentProp.Template.CanStack, 100, 240);

				} GUILayout.EndVertical();


				GUILayout.BeginVertical(GUILayout.Width(100)); {
					CurrentProp.Icon = (Texture2D) EditorGUILayout.ObjectField(CurrentProp.Icon, typeof (Texture2D), false, new GUILayoutOption[] {GUILayout.Width(70), GUILayout.Height(70)});

				} GUILayout.EndVertical();
				GUILayout.EndHorizontal();



				GUIStyle alignment_center = new GUIStyle (EditorStyles.label); 
				alignment_center.alignment = TextAnchor.MiddleCenter;

				GUIStyle alignment_right = new GUIStyle (EditorStyles.label); 
				alignment_right.alignment = TextAnchor.MiddleRight;


				GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Allowed Scale: ", GUILayout.Width (100));

				float minLimit = AssetBundlesSettings.MIN_ALLOWED_SIZE;
				float maxLimit = AssetBundlesSettings.MAX_AlLOWED_SIZE;

				EditorGUILayout.MinMaxSlider (ref CurrentProp.Template.MinSize, ref CurrentProp.Template.MaxSize, minLimit, maxLimit,  GUILayout.Width (240));  //    EditorGUILayout.MinMaxSlider (CurrentProp.Template.MinScale, GUILayout.Width (240));

				if(CurrentProp.Template.MaxSize < CurrentProp.MaxAxisValue) {
					CurrentProp.Template.MaxSize = CurrentProp.MaxAxisValue;
				}

				EditorGUILayout.LabelField (Mathf.CeilToInt(CurrentProp.Template.MinSize * 100f) + "mm / " + Mathf.CeilToInt(CurrentProp.Template.MaxSize * 100f) + "mm", alignment_right, GUILayout.Width (99));
				GUILayout.EndHorizontal ();



				GUILayout.BeginHorizontal ();

				float labelSize = 146;

				Vector3 def = CurrentProp.Size * 100f;
				Vector3 min = def * CurrentProp.MinScale;
				Vector3 max = def * CurrentProp.MaxScale;

			
				EditorGUILayout.LabelField ("Min(" + Mathf.CeilToInt(CurrentProp.MinScale * 100f) + "%): "  + (int)min.x + "x" + (int)min.y + "x" + (int)min.z, GUILayout.Width (labelSize));
				EditorGUILayout.LabelField ("Default: " + (int)def.x + "x" + (int)def.y + "x" + (int)def.z, alignment_center, GUILayout.Width (labelSize));
				GUILayout.Space (1);
				EditorGUILayout.LabelField ("Max(" + Mathf.CeilToInt(CurrentProp.MaxScale * 100f) + "%):" + (int)max.x + "x" + (int)max.y + "x" + (int)max.z, alignment_right, GUILayout.Width (labelSize));



				GUILayout.EndHorizontal ();


			/*	GUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField ("Max Scale: ", GUILayout.Width (100));

				CurrentProp.Template.MaxScale = EditorGUILayout.Slider (CurrentProp.Template.MaxScale, CurrentProp.MaxAxisValue, 4f); // EditorGUILayout.FloatField (CurrentProp.Template.MaxScale, GUILayout.Width (240));
				GUILayout.EndHorizontal ();
*/




				GUILayout.Space (10f);

				GUILayout.BeginHorizontal(); 
				GUILayout.BeginVertical(GUILayout.Width(225)); {

					ReorderableListGUI.Title("Asset Tags");
					ReorderableListGUI.ListField(CurrentProp.Template.Tags, TagListItem, DrawEmptyTag);
				} GUILayout.EndVertical();


				GUILayout.BeginVertical(GUILayout.Width(225)); {

					ReorderableListGUI.Title("Supported Content Types");
					List<string> ContentTypes = new List<string> ();
					foreach(ContentType t in CurrentProp.Template.ContentTypes) {
						ContentTypes.Add (t.ToString ());
					}
					ReorderableListGUI.ListField(ContentTypes, ContentTypeListItem, DrawEmptyContentType);

					CurrentProp.Template.ContentTypes = new List<ContentType> ();
					foreach(string val in ContentTypes) {
						ContentType parsed = SA.Common.Util.General.ParseEnum<ContentType> (val);
						CurrentProp.Template.ContentTypes.Add (parsed);
					}

				} GUILayout.EndVertical();

				GUILayout.EndHorizontal();

			

			} if (EditorGUI.EndChangeCheck ()) {
				//AssetBundlesManager.SavePrefab (CurrentProp);
			}

			GUILayout.BeginHorizontal ();

			GUILayout.FlexibleSpace ();

			Rect buttonRect = new Rect (425, 360, 150, 18);

			if (string.IsNullOrEmpty (CurrentProp.Template.Id)) {
				bool upload = GUI.Button (buttonRect, "Upload");
				if (upload) {
					AssetBundlesManager.UploadAssets (CurrentProp);
				}

			} else {
				bool upload = GUI.Button (buttonRect, "Re Upload");
				if (upload) {
					AssetBundlesManager.ReUploadAsset (CurrentProp);
				}
			}

			GUILayout.Space (40f);
			GUILayout.EndHorizontal ();

				
		}


		public string TagListItem(Rect position, string itemValue) {
			if (itemValue == null)
				itemValue = "new_tag";
			return EditorGUI.TextField(position, itemValue);
		}

		public void DrawEmptyTag() {
			GUILayout.Label("No items in list.", EditorStyles.miniLabel);
		}


		public string ContentTypeListItem(Rect position, string itemValue) {

			position.y += 2;
			if (string.IsNullOrEmpty(itemValue)) {
				itemValue = ContentType.Image.ToString ();
			}

			ContentType t = SA.Common.Util.General.ParseEnum<ContentType> (itemValue);
			t =  (ContentType) EditorGUI.EnumPopup(position, t);
			return t.ToString ();
		}

		public void DrawEmptyContentType() {
			GUILayout.Label("Asset willn't support any content.", EditorStyles.miniLabel);
		}


		//--------------------------------------
		//  Settings
		//--------------------------------------
			
		private void Settings() {


			GUILayout.Space(10f);


			ReorderableListGUI.Title("Build Platfroms");
			List<string> PlatfromsList = new List<string> ();
			foreach (var platform in AssetBundlesSettings.Instance.TargetPlatforms) {
				PlatfromsList.Add (platform.ToString ());
			}
			ReorderableListGUI.ListField(PlatfromsList, PlatformListItem, DrawEmptyPlatform);

			AssetBundlesSettings.Instance.TargetPlatforms = new List<BuildTarget> ();

			foreach(string val in PlatfromsList) {
				BuildTarget parsed = SA.Common.Util.General.ParseEnum<BuildTarget> (val);
				AssetBundlesSettings.Instance.TargetPlatforms.Add (parsed);
			}
		}

		public string PlatformListItem(Rect position, string itemValue) {

			position.y += 2;
			if (string.IsNullOrEmpty(itemValue)) {
				itemValue = BuildTarget.iOS.ToString ();
			}

			BuildTarget t = SA.Common.Util.General.ParseEnum<BuildTarget> (itemValue);
			t =  (BuildTarget) EditorGUI.EnumPopup(position, t);
			return t.ToString ();
		}

		public void DrawEmptyPlatform() {
			GUILayout.Label("Please select atleas one asset platfrom", EditorStyles.miniLabel);
		}

	

		//--------------------------------------
		//  Assets
		//--------------------------------------

		private string SearchField = string.Empty;
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

			SearchField = GUILayout.TextField(SearchField, new GUILayoutOption[] {GUILayout.ExpandWidth(true)});

			Texture2D refreshIcon = Resources.Load ("refresh") as Texture2D;
			bool refresh = GUILayout.Button (refreshIcon, WizzardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if (refresh) {
				List<string> separatedTags = new List<string>(SearchField.Split(' '));
				AssetRequestManager.ReloadAssets (separatedTags);
			}

			bool addnew = GUILayout.Button ("+", WizzardWindow.constants.settingsBoxTitle, GUILayout.Width (20));
			if(addnew) {
				WindowManager.ShowCreateNewAsset ();
			}



			Texture2D trash = Resources.Load ("trash") as Texture2D;
			bool remove = GUILayout.Button (trash, WizzardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if(remove && SelectedAsset != null) {
				if (EditorUtility.DisplayDialog ("Delete " + SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {;
					AssetRequestManager.RemoveAsset (SelectedAsset);
				}
			}



			GUILayout.EndHorizontal ();

			m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, WizzardWindow.constants.settingsBox,  new GUILayoutOption[] {GUILayout.Width(230), GUILayout.Height(310)});
			foreach(var asset in AssetBundlesSettings.Instance.LocalAssetTemplates) {
				/*if(asset.Thumbnail ==  null) {
					asset.RestoreThumbnail ();
				}*/
				if (GUILayout.Toggle(SelectedAsset == asset, asset.DisaplyContent, WizzardWindow.constants.keysElement, new GUILayoutOption[] {GUILayout.Width(230)})) {
					SelectedAsset = asset;
				}
			}

			if (AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
				EditorGUILayout.Space ();

				if(GUILayout.Button ("Load more", EditorStyles.miniButton, GUILayout.Width(60))) {
					List<string> separatedTags = new List<string>(SearchField.Split(' '));
					AssetRequestManager.LoadMoreAssets (separatedTags);
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
				AssetInfoLable ("Size", SelectedAsset.Size);
				AssetInfoLable ("Placing", SelectedAsset.Placing);
				AssetInfoLable ("Invoke", SelectedAsset.InvokeType);
				AssetInfoLable ("Can Stack", SelectedAsset.CanStack);
				AssetInfoLable ("Max Scale", SelectedAsset.MaxSize);
				AssetInfoLable ("Min Scale", SelectedAsset.MinSize);

				string tags = string.Empty;
				foreach(string tag in SelectedAsset.Tags) {
					tags += tag;

					if(SelectedAsset.Tags.IndexOf(tag) == (SelectedAsset.Tags.Count -1)) {
						tags+= ";";
					} else {
						tags+= ", ";
					}

						
				}

				AssetInfoLable ("Tags", tags);


				string types = string.Empty;
				foreach(ContentType t in SelectedAsset.ContentTypes) {
					types += t.ToString ();

					if(SelectedAsset.ContentTypes.IndexOf(t) == (SelectedAsset.ContentTypes.Count -1)) {
						types+= ";";
					} else {
						types+= ", ";
					}
				}

				AssetInfoLable ("Types", types);


				EditorGUILayout.Space ();


				bool edit = GUILayout.Button ("Edit Asset", EditorStyles.miniButton, GUILayout.Width(100));
				if(edit) {
					AssetBundlesManager.LoadAssetBundle (SelectedAsset);
				}

			}


			GUILayout.EndVertical();

			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

		}


		private void AssetInfoLable(string title, object msg) {
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField (title + ": ",  EditorStyles.boldLabel, new GUILayoutOption[] {GUILayout.Height(16), GUILayout.Width(65)});
			EditorGUILayout.SelectableLabel (msg.ToString(), EditorStyles.label, new GUILayoutOption[] {GUILayout.Height(16)});
			GUILayout.EndHorizontal ();
		}
			


		//--------------------------------------
		//  Account
		//--------------------------------------

		private void Account() {

			if (string.IsNullOrEmpty (AssetBundlesSettings.Instance.SessionId)) {
				GUILayout.Label ("Use your Roomful account email and password to sign in.");

				AuthWindow ();
				return;
			}


			GUILayout.Label ("Roomful asset wizzard. Logged as: "+AssetBundlesSettings.Instance.SessionId);

			if (GUILayout.Button ("Log Out")) {
				Mail = string.Empty;
				Password = string.Empty;

				AssetBundlesSettings.Instance.SetSessionId(string.Empty);
			}
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
		// Private Methods
		//--------------------------------------

		private static void DirtyEditor() {

		}



		private static bool YesNoFiled(string title, bool value, int width1, int width2) {

			SA.Common.Editor.SA_YesNoBool initialValue = SA.Common.Editor.SA_YesNoBool.Yes;
			if(!value) {
				initialValue = SA.Common.Editor.SA_YesNoBool.No;
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(title,  GUILayout.Width (width1));

			initialValue = (SA.Common.Editor.SA_YesNoBool) EditorGUILayout.EnumPopup(initialValue, GUILayout.Width (width2));
			if(initialValue == SA.Common.Editor.SA_YesNoBool.Yes) {
				value = true;
			} else {
				value = false;
			}
			EditorGUILayout.EndHorizontal();

			return value;
		}

	}
}
