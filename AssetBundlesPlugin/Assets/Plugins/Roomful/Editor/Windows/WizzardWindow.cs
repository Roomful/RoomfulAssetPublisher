using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor {
	public class WizardWindow : EditorWindow {

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
            public GUIStyle keysElement = "PreferencesKeysElement";
			public GUIStyle warningIcon = "CN EntryWarn";
			public GUIStyle sectionHeader = new GUIStyle(EditorStyles.largeLabel);
			public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);
			public GUIStyle toolbarStyle;
			public GUIStyle toolbarSeachTextFieldStyle;
			public GUIStyle toolbarSeachCancelButtonStyle;

			public Constants() {
				sectionHeader = new GUIStyle(EditorStyles.largeLabel);

				sectionScrollView = new GUIStyle(this.sectionScrollView);
				sectionScrollView.overflow.bottom++;


				toolbarStyle 					= GUI.skin.FindStyle("Toolbar");
				toolbarSeachTextFieldStyle 	= GUI.skin.FindStyle("ToolbarSeachTextField");
				toolbarSeachCancelButtonStyle 	= GUI.skin.FindStyle("ToolbarSeachCancelButton");


				sectionHeader.fontStyle = FontStyle.Bold;
				sectionHeader.fontSize = 18;
				sectionHeader.margin.top = 10;
				sectionHeader.margin.left++;

  
                if (!EditorGUIUtility.isProSkin) {
					sectionHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
				} else {
					sectionHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
				}

				cacheFolderLocation.wordWrap = true;
			}
		}


		private delegate void OnGUIDelegate();

		private class Section {
			public GUIContent content;

			public WizardWindow.OnGUIDelegate guiFunc;

			public Section(string name, WizardWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name);
				this.guiFunc = guiFunc;
			}

			public Section(string name, Texture2D icon, WizardWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name, icon);
				this.guiFunc = guiFunc;
			}

			public Section(GUIContent content, WizardWindow.OnGUIDelegate guiFunc) {
				this.content = content;
				this.guiFunc = guiFunc;
			}
		}



		private Vector2 m_SectionScrollPos;
		private List<WizardWindow.Section> m_Sections;
		private static WizardWindow.Constants constants = null;



		//--------------------------------------
		//  Initialisation
		//--------------------------------------


		private void OnEnable() {
			this.m_Sections = new List<WizardWindow.Section>();
			this.m_Sections.Add(new WizardWindow.Section("Wizzard", new WizardWindow.OnGUIDelegate(this.Wizard)));
			this.m_Sections.Add(new WizardWindow.Section("Assets", new WizardWindow.OnGUIDelegate(this.Assets)));
			this.m_Sections.Add(new WizardWindow.Section("Settings", new WizardWindow.OnGUIDelegate(this.Settings)));
			this.m_Sections.Add(new WizardWindow.Section("Account", new WizardWindow.OnGUIDelegate(this.Account)));
		}


		//--------------------------------------
		//  GUI Render
		//--------------------------------------

		void OnGUI() {

			GUI.changed = false;
			EditorGUIUtility.labelWidth = 200f;

			if (WizardWindow.constants == null) {
				WizardWindow.constants = new WizardWindow.Constants();
			}


            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = 3;
            }

              


            GUILayout.BeginHorizontal(new GUILayoutOption[0]);


			m_SectionScrollPos = GUILayout.BeginScrollView(this.m_SectionScrollPos, WizardWindow.constants.sectionScrollView, new GUILayoutOption[]{ GUILayout.Width(120f)});

			GUILayout.Space(40f);
			for (int i = 0; i < this.m_Sections.Count; i++) {
				WizardWindow.Section section = this.m_Sections[i];

				Rect rect = GUILayoutUtility.GetRect(section.content, WizardWindow.constants.sectionElement, new GUILayoutOption[]{GUILayout.ExpandWidth(true)});

				if (section == this.selectedSection && Event.current.type == EventType.Repaint) {

                    Color color;
                    if (EditorGUIUtility.isProSkin) {
                        color = new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f);
                    } else {
                        color = new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);
                    }

                    GUI.DrawTexture(rect, IconManager.GetIcon(color));
				}

				EditorGUI.BeginChangeCheck();
				if (GUI.Toggle(rect, AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex == i, section.content, WizardWindow.constants.sectionElement)) {
                    AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = i;
				} if (EditorGUI.EndChangeCheck()){
					GUIUtility.keyboardControl = 0;
				}
			}


            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(WizardWindow.constants.settingsBoxTitle);
            {
                GUILayout.FlexibleSpace();
                bool platfromClick = GUILayout.Button(EditorUserBuildSettings.activeBuildTarget.ToString(), EditorStyles.label);
                if (platfromClick) {
                    SiwtchTab(WizardTabs.Platforms);
                }
                GUILayout.FlexibleSpace();
            } GUILayout.EndHorizontal();


            GUILayout.EndScrollView();
			GUILayout.Space(10f);

			GUILayout.BeginVertical(new GUILayoutOption[0]);
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

		private WizardWindow.Section selectedSection {
			get {
				return this.m_Sections[AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex];
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

		public void SiwtchTab(WizardTabs tab) {
            AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = (int)tab;
		}


		//--------------------------------------
		//  Wizzard
		//--------------------------------------


		private void Wizard() {
           
            GUILayout.Label("Wizard", WizardWindow.constants.sectionHeader, new GUILayoutOption[0]);

            if (AssetBundlesSettings.Instance.IsUploadInProgress) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }

            bool GUIState = GUI.enabled;

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
					GUI.enabled = GUIState;
					GUILayout.EndHorizontal ();

					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Placing: ", GUILayout.Width (100));
					CurrentProp.Template.Placing = (Placing) EditorGUILayout.EnumPopup(CurrentProp.Template.Placing, GUILayout.Width (240));
					GUILayout.EndHorizontal ();


					GUILayout.BeginHorizontal ();
					EditorGUILayout.LabelField ("Invoke Type: ", GUILayout.Width (100));
					CurrentProp.Template.InvokeType = (InvokeTypes) EditorGUILayout.EnumPopup(CurrentProp.Template.InvokeType, GUILayout.Width (240));
					GUILayout.EndHorizontal ();

					if(CurrentProp.HasStandSurface) {
						CurrentProp.Template.CanStack = false;
						GUI.enabled = GUIState;
					}
						
					CurrentProp.Template.CanStack = YesNoFiled ("CanStack", CurrentProp.Template.CanStack, 100, 240);
					GUI.enabled = GUIState;

				} GUILayout.EndVertical();


				GUILayout.BeginVertical(GUILayout.Width(100)); {
                    CurrentProp.Icon = (Texture2D)EditorGUILayout.ObjectField(CurrentProp.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });


                    if(CurrentProp.Icon == null) {
                         DrawPreloaderAt(new Rect(525, 65, 32, 32));
                    }
					
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

			Rect buttonRect1 = new Rect (460, 360, 120, 18);
			Rect buttonRect2 = new Rect (310, 360, 120, 18);

			Rect buttonRect3 = new Rect (460, 390, 120, 18);

			if (string.IsNullOrEmpty (CurrentProp.Template.Id)) {
				bool upload = GUI.Button (buttonRect1, "Upload");
				if (upload) {
					AssetBundlesSettings.Instance.IsInAutoloading = false;

					AssetBundlesManager.UploadAssets (CurrentProp);
				}

			} else {
				bool upload = GUI.Button (buttonRect1, "Re Upload");
				if (upload) {
					AssetBundlesSettings.Instance.IsInAutoloading = false;

					AssetBundlesManager.ReuploadAsset (CurrentProp);
				}

				bool refresh = GUI.Button (buttonRect2, "Refresh");
				if (refresh) {
					AssetBundlesManager.DownloadAssetBundle (CurrentProp.Template);
				}
			}

			bool create = GUI.Button (buttonRect3, "Create New");
			if (create) {
				WindowManager.ShowCreateNewAsset ();
			}


			GUILayout.Space (40f);
			GUILayout.EndHorizontal ();


            GUI.enabled = true;
            Repaint();
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


            GUILayout.Label("Settings", WizardWindow.constants.sectionHeader, new GUILayoutOption[0]);

            GUILayout.Space(10f);


			EditorGUI.BeginChangeCheck ();


			ReorderableListGUI.Title("Build Platfroms");
			List<string> PlatfromsList = new List<string> ();
			foreach (var platform in AssetBundlesSettings.Instance.TargetPlatforms) {
				PlatfromsList.Add (platform.ToString ());
			}

			ReorderableListGUI.ListField(PlatfromsList, DrawPlatformListItem, DrawEmptyPlatform);

			AssetBundlesSettings.Instance.TargetPlatforms = new List<BuildTarget> ();

			foreach(string val in PlatfromsList) {
				BuildTarget parsed = SA.Common.Util.General.ParseEnum<BuildTarget> (val);
				AssetBundlesSettings.Instance.TargetPlatforms.Add (parsed);
			}
            
			ReorderableListGUI.Title("Plugin Settings");
			GUILayout.Space(10f);
			AssetBundlesSettings.Instance.ShowWebInLogs = SA.Common.Editor.Tools.YesNoFiled ("WEB IN Logs", AssetBundlesSettings.Instance.ShowWebInLogs);
			AssetBundlesSettings.Instance.ShowWebOutLogs = SA.Common.Editor.Tools.YesNoFiled ("WEB OUT Logs", AssetBundlesSettings.Instance.ShowWebOutLogs);
            AssetBundlesSettings.Instance.AutomaticCacheClean = SA.Common.Editor.Tools.YesNoFiled("Automatic Cache Clean", AssetBundlesSettings.Instance.AutomaticCacheClean);

            //AssetBundlesSettings.Instance.PublisherCurrentVersionIndex = EditorGUILayout.Popup("Version: ", AssetBundlesSettings.Instance.PublisherCurrentVersionIndex, AssetBundlesSettings.Instance.PublisherExistingVersions);

            if (EditorGUI.EndChangeCheck()) {
				AssetBundlesSettings.Save ();
			}

            GUILayout.Space(10f);
            ReorderableListGUI.Title("Actions");
            GUILayout.Space(10f);

            //if (GUILayout.Button("Reupload all assets")) {
               //AutomaticReloader.ReloadAllAssets();
            //}

            if (GUILayout.Button("Clear local cache")) {
                AssetBundlesManager.ClearLocalCache();
            }

        }

        public string DrawPlatformListItem(Rect position, string itemValue) {

			position.y += 2;
			if (string.IsNullOrEmpty(itemValue)) {
				itemValue = BuildTarget.iOS.ToString ();
			}

            position.width -= 25;

            BuildTarget buildTraget = SA.Common.Util.General.ParseEnum<BuildTarget> (itemValue);
			buildTraget =  (BuildTarget) EditorGUI.EnumPopup(position, buildTraget);


            position.x += position.width + 2;
            position.width = 20;
            position.height = 15;


            GUIContent buttonContent = new GUIContent();
            buttonContent.image = IconManager.GetIcon(Icon.refresh_black);


            if (EditorUserBuildSettings.activeBuildTarget == buildTraget) {
                GUI.enabled = false;
            }

            bool switchPlatfrom = GUI.Button(position, buttonContent, EditorStyles.miniButton);
            if (switchPlatfrom) {

                BuildTargetGroup group = BuildTargetGroup.Unknown;
                switch(buildTraget) {
                    case BuildTarget.iOS:
                        group = BuildTargetGroup.iOS;
                        break;
                    case BuildTarget.WebGL:
                        group = BuildTargetGroup.WebGL;
                        break;
                }

                EditorUserBuildSettings.SwitchActiveBuildTargetAsync(group, buildTraget);
            }

            GUI.enabled = true;

            return buildTraget.ToString ();
		}

		public void DrawEmptyPlatform() {
			GUILayout.Label("Please select atleas one asset platfrom", EditorStyles.miniLabel);
		}
        
		//--------------------------------------
		//  Assets
		//--------------------------------------

		private Vector2 m_KeyScrollPos;
		private AssetTemplate SelectedAsset = null;
		private const string SEARTCH_BAR_CONTROL_NAME = "seartchBat";
        
        int RotationAnimatorAgnle = 0;
        private void DrawPreloaderAt(Rect rect) {
            Texture2D preloader = IconManager.GetIcon(Icon.loader); 

            RotationAnimatorAgnle++;

            if (RotationAnimatorAgnle > 360) {
                RotationAnimatorAgnle = 0;
            }

            GUIUtility.RotateAroundPivot(RotationAnimatorAgnle, rect.center);
            GUI.DrawTexture(rect, preloader);
            GUI.matrix = Matrix4x4.identity;
        }

        private int m_itemsPreloaderAgnle = 0;
        private void Assets() {

            GUILayout.Label("Assets", WizardWindow.constants.sectionHeader, new GUILayoutOption[0]);
            
            if (!AssetBundlesSettings.Instance.LocalAssetTemplates.Contains(SelectedAsset)) {
				SelectedAsset = null;
			}

			if (SelectedAsset == null) {
				if(AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
					SelectedAsset = AssetBundlesSettings.Instance.LocalAssetTemplates [0];
				}
			}

            if(RequestManager.ASSETS_SEARTCH_IN_PROGRESS) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }
			
			GUILayout.BeginHorizontal(WizardWindow.constants.settingsBoxTitle); {
                
				GUIStyle s = new GUIStyle (EditorStyles.boldLabel);
				s.margin = new RectOffset (0, 0, 0, 0);
				s.padding = new RectOffset (2, 2, 2, 2);

				GUILayout.Label("Your Assets List", s, new GUILayoutOption[] {GUILayout.Width(130)});
				AssetBundlesSettings.Instance.SeartchType = (SeartchRequestType) EditorGUILayout.EnumPopup(AssetBundlesSettings.Instance.SeartchType, GUILayout.Width (55));


				GUI.SetNextControlName(SEARTCH_BAR_CONTROL_NAME);
				AssetBundlesSettings.Instance.SeartchPattern = EditorGUILayout.TextField(AssetBundlesSettings.Instance.SeartchPattern, WizardWindow.constants.toolbarSeachTextFieldStyle, GUILayout.MinWidth(150));

				if (GUILayout.Button("", WizardWindow.constants.toolbarSeachCancelButtonStyle)) {
					AssetBundlesSettings.Instance.SeartchPattern = string.Empty;
					GUI.FocusControl(null);
				}

                Texture2D refreshIcon = IconManager.GetIcon(Icon.refresh_black);
				bool refresh = GUILayout.Button (refreshIcon, WizardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
				if (refresh) {
					AssetBundlesSettings.Instance.LocalAssetTemplates.Clear ();
					RequestManager.SeartchAssets ();
				}

				bool addnew = GUILayout.Button ("+", WizardWindow.constants.settingsBoxTitle, GUILayout.Width (20));
				if(addnew) {
					WindowManager.ShowCreateNewAsset ();
				}
					


				GUILayout.Space (7);
			} GUILayout.EndHorizontal();

			GUILayout.Space (1);



			int ASSETS_LIST_WIDTH = 200;
			int ASSETS_INFO_WIDTH = 268;

			int SCROLL_BAR_HEIGHT = 350;

			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical( GUILayout.Width(ASSETS_LIST_WIDTH));
		
			GUI.Box (new Rect (130, 58, ASSETS_LIST_WIDTH, SCROLL_BAR_HEIGHT), "", WizardWindow.constants.settingsBox);

			m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none,  GUI.skin.verticalScrollbar, new GUILayoutOption[] {GUILayout.Width(ASSETS_LIST_WIDTH), GUILayout.Height(SCROLL_BAR_HEIGHT)});


            m_itemsPreloaderAgnle+= 3;
            if (m_itemsPreloaderAgnle > 360) {
                m_itemsPreloaderAgnle = 0;
            }

            foreach (var asset in AssetBundlesSettings.Instance.LocalAssetTemplates) {

                GUIContent assetDisaplyContent = asset.DisaplyContent;

                if (assetDisaplyContent.image == null) {
                    Texture2D preloader = IconManager.Rotate(IconManager.GetIcon(Icon.loader), m_itemsPreloaderAgnle);
                    assetDisaplyContent.image = preloader;
                }

                if (GUILayout.Toggle(SelectedAsset == asset, assetDisaplyContent, WizardWindow.constants.keysElement, new GUILayoutOption[] {GUILayout.Width(ASSETS_LIST_WIDTH)})) {
					SelectedAsset = asset;
				}

            }

			if (AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
				EditorGUILayout.Space ();

				if(GUILayout.Button ("Load more", EditorStyles.miniButton, GUILayout.Width(65))) {
					RequestManager.SeartchAssets ();
				}
			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();


		



			GUILayout.BeginVertical(GUILayout.Width(ASSETS_INFO_WIDTH));



			if(SelectedAsset != null) {

				GUILayout.BeginHorizontal ();


				GUILayout.Label("Selected Asset", WizardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(ASSETS_INFO_WIDTH - 20*2)});


				Texture2D edit = Resources.Load ("edit") as Texture2D;
				bool editAsset = GUILayout.Button (edit, WizardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
				if(editAsset) {
					AssetBundlesManager.DownloadAssetBundle (SelectedAsset);
				}



				Texture2D trash = Resources.Load ("trash") as Texture2D;
				bool removeAsset = GUILayout.Button (trash, WizardWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
				if(removeAsset) {
					if (EditorUtility.DisplayDialog ("Delete " + SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {;
						RequestManager.RemoveAsset (SelectedAsset);
					}
				}


				GUILayout.EndHorizontal ();

				EditorGUILayout.Space ();

				AssetInfoLable ("Id", SelectedAsset.Id);
				AssetInfoLable ("Title", SelectedAsset.Title);
				AssetInfoLable ("Size", SelectedAsset.Size);
				AssetInfoLable ("Placing", SelectedAsset.Placing);
				AssetInfoLable ("Invoke", SelectedAsset.InvokeType);
				AssetInfoLable ("Can Stack", SelectedAsset.CanStack);
				AssetInfoLable ("Max Scale", SelectedAsset.MaxSize);
				AssetInfoLable ("Min Scale", SelectedAsset.MinSize);


				string Plaforms = string.Empty;
				foreach(AssetUrl p in SelectedAsset.Urls) {
					Plaforms += p.Platform + "  ";
				}
				AssetInfoLable ("Plaforms", Plaforms);


				//Types
				string types = string.Empty;
				foreach(ContentType t in SelectedAsset.ContentTypes) {
					types += t.ToString ();

					if(SelectedAsset.ContentTypes.IndexOf(t) == (SelectedAsset.ContentTypes.Count -1)) {
						types+= ";";
					} else {
						types+= ", ";
					}
				}

				if(types.Equals(string.Empty)) {
					types = "None;";
				}

				AssetInfoLable ("Types", types);


				//Tags
				int countBeforeBreake = 0;
				int line = 0;
			
				List<string> tags = new List<string>();
				tags.Add (string.Empty);

				foreach(string tag in SelectedAsset.Tags) {

					if(countBeforeBreake == 3) {
						countBeforeBreake = 0;
						line++;
						tags.Add (string.Empty);
					}

					tags[line] += tag;

					if(SelectedAsset.Tags.IndexOf(tag) == (SelectedAsset.Tags.Count -1)) {
						tags[line]+= ";";
					} else {
						tags[line]+= ", ";
					}

					countBeforeBreake++;
						
				}

				for(int i = 0; i < tags.Count; i++) {
					if(i == 0) {
						AssetInfoLable ("Tags", tags[i]);
					} else {
						AssetInfoLable (string.Empty, tags[i]);
					}
				}



				AssetInfoLable ("Created", SelectedAsset.Created.ToString());
				AssetInfoLable ("Updated", SelectedAsset.Updated.ToString());
			

				EditorGUILayout.Space ();



			}


			GUILayout.EndVertical();

			GUILayout.Space(10f);
			GUILayout.EndHorizontal();


			Texture2D roomful_logo = Resources.Load ("roomful_logo") as Texture2D;
			GUI.DrawTexture (new Rect (380, 358, roomful_logo.width, roomful_logo.height), roomful_logo);



            GUI.enabled = true;


            this.Repaint();

        }
			

		private void AssetInfoLable(string title, object msg) {
			GUILayout.BeginHorizontal();

			if(!string.IsNullOrEmpty(title)) {
				title += ": ";
			}

			EditorGUILayout.LabelField (title,  EditorStyles.boldLabel, new GUILayoutOption[] {GUILayout.Height(16), GUILayout.Width(65)});
			EditorGUILayout.SelectableLabel (msg.ToString(), EditorStyles.label, new GUILayoutOption[] {GUILayout.Height(16)});
			GUILayout.EndHorizontal ();
		}
			


		//--------------------------------------
		//  Account
		//--------------------------------------

		private void Account() {

            GUILayout.Label("Account", WizardWindow.constants.sectionHeader, new GUILayoutOption[0]);

            if (AssetBundlesSettings.Instance.IsLoggedIn) {
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
			Password = EditorGUILayout.PasswordField ("Password: ", Password);

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
          //  AssetBundlesSettings.Save();
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
