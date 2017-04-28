////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.UltimateLogger {


	public class PreferencesWindow : EditorWindow {


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

			public PreferencesWindow.OnGUIDelegate guiFunc;

			public Section(string name, PreferencesWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name);
				this.guiFunc = guiFunc;
			}

			public Section(string name, Texture2D icon, PreferencesWindow.OnGUIDelegate guiFunc) {
				this.content = new GUIContent(name, icon);
				this.guiFunc = guiFunc;
			}

			public Section(GUIContent content, PreferencesWindow.OnGUIDelegate guiFunc) {
				this.content = content;
				this.guiFunc = guiFunc;
			}
		}




		private int m_SelectedSectionIndex;
		private Vector2 m_SectionScrollPos;
		private List<PreferencesWindow.Section> m_Sections;
		private static PreferencesWindow.Constants constants = null;


		private void OnEnable() {


			this.m_Sections = new List<PreferencesWindow.Section>();
			this.m_Sections.Add(new PreferencesWindow.Section("General", new PreferencesWindow.OnGUIDelegate(this.ShowGeneral)));
			this.m_Sections.Add(new PreferencesWindow.Section("Toolbar", new PreferencesWindow.OnGUIDelegate(this.Toolbar)));
			this.m_Sections.Add(new PreferencesWindow.Section("Platfroms", new PreferencesWindow.OnGUIDelegate(this.Platfroms)));
			this.m_Sections.Add(new PreferencesWindow.Section("Console Tags", new PreferencesWindow.OnGUIDelegate(this.Tags)));
			this.m_Sections.Add(new PreferencesWindow.Section("Ignore List", new PreferencesWindow.OnGUIDelegate(this.IgnoreList)));
			this.m_Sections.Add(new PreferencesWindow.Section("About", new PreferencesWindow.OnGUIDelegate(this.ShowGeneral)));

			Settings.Init ();

		}




		void OnGUI() {

			GUI.changed = false;
			EditorGUIUtility.labelWidth = 200f;

			if (PreferencesWindow.constants == null) {
				PreferencesWindow.constants = new PreferencesWindow.Constants();
			}



			GUILayout.BeginHorizontal(new GUILayoutOption[0]);

			m_SectionScrollPos = GUILayout.BeginScrollView(this.m_SectionScrollPos, PreferencesWindow.constants.sectionScrollView, new GUILayoutOption[]{ GUILayout.Width(120f)});

			GUILayout.Space(40f);
			for (int i = 0; i < this.m_Sections.Count; i++) {
				PreferencesWindow.Section section = this.m_Sections[i];

				Rect rect = GUILayoutUtility.GetRect(section.content, PreferencesWindow.constants.sectionElement, new GUILayoutOption[]{GUILayout.ExpandWidth(true)});
				
				if (section == this.selectedSection && Event.current.type == EventType.Repaint) {
					PreferencesWindow.constants.selected.Draw(rect, false, false, false, false);
				}

				EditorGUI.BeginChangeCheck();
				if (GUI.Toggle(rect, this.selectedSectionIndex == i, section.content, PreferencesWindow.constants.sectionElement)) {
					this.selectedSectionIndex = i;
				} if (EditorGUI.EndChangeCheck()){
					GUIUtility.keyboardControl = 0;
				}
			}


			GUILayout.EndScrollView();
			GUILayout.Space(10f);

			GUILayout.BeginVertical(new GUILayoutOption[0]);
			GUILayout.Label(this.selectedSection.content, PreferencesWindow.constants.sectionHeader, new GUILayoutOption[0]);
			this.selectedSection.guiFunc();
			GUILayout.Space(5f);
			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

			if(GUI.changed) {
				DirtyEditor();
			}
		}



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


		private PreferencesWindow.Section selectedSection {
			get {
				return this.m_Sections[this.m_SelectedSectionIndex];
			}
		}

		private static LoggerSettings Settings {
			get {
				return LoggerSettings.Instance;
			}
		}


		private static LoggerPlatfromsSettings PlatfromsSettings {
			get {
				return LoggerPlatfromsSettings.Instance;
			}
		}

		private void ShowGeneral() {
			GUILayout.Space(10f);

			EditorGUI.BeginChangeCheck();

			Settings.fontSize = EditorGUILayout.IntField ("Font Size", Settings.fontSize);
			Settings.logLinePadding = EditorGUILayout.IntField ("Line Padding", Settings.logLinePadding);


			Settings.ShowTagInMessageLine = UL_Tools.ToggleFiled ("Show Tag Names", Settings.ShowTagInMessageLine);

			if (EditorGUI.EndChangeCheck()) {
				LoggerWindow.RefreshUI ();
			}

			GUILayout.FlexibleSpace ();
			bool restoreDefaults = GUILayout.Button ("Restore Defaults", GUILayout.Width(150));
			if(restoreDefaults) {
				Settings.fontSize = 11;
				Settings.logLinePadding = 2;
				Settings.ShowTagInMessageLine = false;

				GUI.FocusControl(null);
				LoggerWindow.RefreshUI ();
			}
		}

		private void Toolbar() {
			GUILayout.Space(10f);

			EditorGUI.BeginChangeCheck();

			Settings.DisplayCollapse = UL_Tools.ToggleFiled ("Collapse Button", Settings.DisplayCollapse);
			Settings.DisplayClearOnPlay = UL_Tools.ToggleFiled ("Clear On Play Button", Settings.DisplayClearOnPlay);
			Settings.DisplayPauseOnError = UL_Tools.ToggleFiled ("Error Pause Button", Settings.DisplayPauseOnError);

			Settings.DisplaySeartchBar = UL_Tools.ToggleFiled ("Seartch Bar", Settings.DisplaySeartchBar);
			Settings.DisplayTagsBar = UL_Tools.ToggleFiled ("Tags Bar", Settings.DisplayTagsBar);


			if (EditorGUI.EndChangeCheck()) {
				LoggerWindow.RefreshUI ();
			}


			GUILayout.FlexibleSpace ();
			bool restoreDefaults = GUILayout.Button ("Restore Defaults", GUILayout.Width(150));
			if(restoreDefaults) {
				Settings.DisplayClearOnPlay = true;
				Settings.DisplayCollapse = true;
				Settings.DisplayPauseOnError = true;

				Settings.DisplaySeartchBar = true;
				Settings.DisplayTagsBar = true;

				GUI.FocusControl(null);
				LoggerWindow.RefreshUI ();
			}


		}

		private void Platfroms() {
			GUILayout.Space(10f);


			GUILayout.Label ("iOS", EditorStyles.boldLabel);

			PlatfromsSettings.iOS_LogsRecord = UL_Tools.ToggleFiled ("Logs Record", PlatfromsSettings.iOS_LogsRecord);
			PlatfromsSettings.iOS_OverrideLogsOutput = UL_Tools.ToggleFiled ("Override XCode Output", PlatfromsSettings.iOS_OverrideLogsOutput);



			GUILayout.Space (5f);

			GUILayout.Label ("Android", EditorStyles.boldLabel);

			PlatfromsSettings.Android_LogsRecord = UL_Tools.ToggleFiled ("Logs Record", PlatfromsSettings.Android_LogsRecord);
			PlatfromsSettings.Android_OverrideLogsOutput = UL_Tools.ToggleFiled ("Override LogCat Output", PlatfromsSettings.Android_OverrideLogsOutput);

		}



		private Vector2 m_KeyScrollPos;
		private CustomTag SelectedTag = null;
		private void Tags() {
			//SelectedTag = Settings.Tags [0];

			if(!Settings.Tags.Contains(SelectedTag)) {
				SelectedTag = null;
			}

			if(SelectedTag == null) {
				if(Settings.Tags.Count > 0) {
					SelectedTag = Settings.Tags [0];
				}
			}

			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();



			GUILayout.BeginVertical( GUILayout.Width(160));

			GUILayout.BeginHorizontal ();
			GUILayout.Label("Predefined Tags", PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.ExpandWidth(true)});
			bool addnew = GUILayout.Button ("+", PreferencesWindow.constants.settingsBoxTitle, GUILayout.Width (20));
			if(addnew) {
				var tag = new CustomTag ();
				tag.Name = "new_tag";
				tag.Icon =  Resources.Load ("icons/tag") as Texture2D; 
				Settings.Tags.Add (tag);
			}

			if(SelectedTag.Name.Equals(LoggerSettings.MESSAGE_TAG_NAME) || SelectedTag.Name.Equals(LoggerSettings.WARNING_TAG_NAME) ||SelectedTag.Name.Equals(LoggerSettings.ERROR_TAG_NAME)) {
				GUI.enabled = false;
			} 

			Texture2D trash = Resources.Load ("icons/trash") as Texture2D;
			bool remove = GUILayout.Button (trash, PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if(remove) {
				Settings.Tags.Remove (SelectedTag);
			}

			GUI.enabled = true;
				
			GUILayout.EndHorizontal ();

			m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, PreferencesWindow.constants.settingsBox);
			foreach(var tag in Settings.Tags) {

				Color oldColor = GUI.color;
				if(SelectedTag != tag && !tag.Docked) {
					GUI.color = Color.black;
				}

				if (GUILayout.Toggle(SelectedTag == tag, tag.DisaplyContent, PreferencesWindow.constants.keysElement, new GUILayoutOption[0])) {
					SelectedTag = tag;
				}

				GUI.color = oldColor;


			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();




			EditorGUI.BeginChangeCheck();
			GUILayout.BeginVertical(GUILayout.Width(230));


			if(SelectedTag != null) {


				GUILayout.Label ("Tag Options", EditorStyles.boldLabel);
				GUILayout.Space (5f);

				if(SelectedTag.Name.Equals(LoggerSettings.MESSAGE_TAG_NAME) || SelectedTag.Name.Equals(LoggerSettings.WARNING_TAG_NAME) ||SelectedTag.Name.Equals(LoggerSettings.ERROR_TAG_NAME)) {
					GUI.enabled = false;
				} 


				GUILayout.Label ("Tag Name", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				GUILayout.Space (30f);
				SelectedTag.Name =   EditorGUILayout.TextField (SelectedTag.Name, GUILayout.Width(150));
				GUILayout.EndHorizontal ();

				GUI.enabled = true;



				GUILayout.Label ("Tag Icon", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				GUILayout.Space (30f);
				SelectedTag.Icon  = (Texture2D) EditorGUILayout.ObjectField(SelectedTag.Icon, typeof (Texture2D), false, new GUILayoutOption[] {GUILayout.Width(150), GUILayout.Height(15)} ); 
				GUILayout.EndHorizontal ();



				GUILayout.Label ("Is Docked", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				GUILayout.Space (30f);
				SelectedTag.Docked = EditorGUILayout.Toggle (SelectedTag.Docked);
				GUILayout.EndHorizontal ();


				GUILayout.Label ("Is Enabled", EditorStyles.boldLabel);
				GUILayout.BeginHorizontal();
				GUILayout.Space (30f);
				SelectedTag.Enabled= EditorGUILayout.Toggle (SelectedTag.Enabled);
				GUILayout.EndHorizontal ();



			}

			if (EditorGUI.EndChangeCheck()) {
				LoggerWindow.RefreshUI ();
			}

			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

			GUILayout.Space(5f);
			bool restoreDefaults = GUILayout.Button ("Restore Defaults", GUILayout.Width(150));
			if(restoreDefaults) {
				Settings.Tags.Clear (); 
				Settings.InitDefaultTags ();
				LoggerWindow.RefreshUI ();
			}

		}

		private Vector2 m_KeyIgnoreScrollPos;
		private string SelectedWrapper = string.Empty;
		private void IgnoreList() {
			

			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();



			GUILayout.BeginVertical( GUILayout.Width(360));

			GUILayout.BeginHorizontal ();
			GUILayout.Label("Ignored Classes", PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.ExpandWidth(true)});

		

			Texture2D trash = Resources.Load ("icons/trash") as Texture2D;
			bool remove = GUILayout.Button (trash, PreferencesWindow.constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
			if(remove) {
				Settings.IgnoredWrapperClasses.Remove (SelectedWrapper);
				LoggerWindow.Refresh ();
			}
				

			GUILayout.EndHorizontal ();

			m_KeyIgnoreScrollPos = GUILayout.BeginScrollView(m_KeyIgnoreScrollPos, PreferencesWindow.constants.settingsBox);
			foreach(var wrapper in Settings.IgnoredWrapperClasses) {

				Color oldColor = GUI.color;
				if(SelectedWrapper != wrapper) {
					GUI.color = Color.black;
				}

				if (GUILayout.Toggle(SelectedWrapper == wrapper, wrapper, PreferencesWindow.constants.keysElement, new GUILayoutOption[0])) {
					SelectedWrapper = wrapper;
				}

				GUI.color = oldColor;


			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

			GUILayout.Space(5f);
			bool restoreDefaults = GUILayout.Button ("Restore Defaults", GUILayout.Width(150));
			if(restoreDefaults) {
				Settings.IgnoredWrapperClasses.Clear (); 
				LoggerWindow.Refresh ();
			}

		}
	


		private static void DirtyEditor() {

			Settings.Save ();
			PlatfromsSettings.Save ();

		}


	}

}