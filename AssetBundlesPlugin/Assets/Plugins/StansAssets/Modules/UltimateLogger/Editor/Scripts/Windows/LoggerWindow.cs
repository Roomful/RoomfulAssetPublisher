////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @authot Alex Yaremenko (Stan's Assets) 
// @authot Victor Krasivskiy (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace SA.UltimateLogger {

	public class LoggerWindow : EditorWindow, IHasCustomMenu {


		private static LoggerWindow _Instance = null;


		private static GUIStyle EntryStyleBackEven 	= null;
		private static GUIStyle EntryStyleBackOdd 	= null;
		private static GUIStyle EntryStyleSelected 	= null;

		private static GUIStyle EntryStackLienView 	= null;
		private static GUIStyle EntryStackLienSelected 	= null;

		private static GUIStyle EntryDetailedView 	= null;




		private static GUIStyle ToolbarStyle 	= null;
		private static GUIStyle ToolbarSeachTextFieldStyle 	= null;
		private static GUIStyle ToolbarSeachCancelButtonStyle 	= null;


		private float currentScrollViewHeight;
		private bool resize = false;
		private Rect cursorChangeRect;

		private Vector2 logLineContentSize = Vector2.zero;


		private const float RESIZE_LINE_OFFSET = 18f;
		private const float DOUBLE_CLICK_TIME = 0.3f;
		private const string SEARTCH_BAR_CONTROL_NAME = "seartchBat";



		private List<LogInfo> Content = new List<LogInfo>();
		private List<LogInfo> ProcessedLogs = new List<LogInfo>();

		private static Dictionary<string, int> _TagsMessagesCount =  new Dictionary<string, int>();
		private static Dictionary<string, TagSwitchHandler> _TagsSwitches =  new Dictionary<string, TagSwitchHandler>();



		//--------------------------------------
		// Initialisation
		//--------------------------------------


		void OnEnable() {

			_Instance = this;

			titleContent.text = "Console";
			titleContent.image =  Resources.Load ("console/logger_window_icon") as Texture2D;

			currentScrollViewHeight = this.position.height/2;
			cursorChangeRect = new Rect(0, currentScrollViewHeight + RESIZE_LINE_OFFSET, this.position.width, 10f);

			UpdateContent ();
		}

		//--------------------------------------
		// Drawing
		//--------------------------------------

		void OnGUI() {

			SetUpView ();


			DrawToolbar();
			ResizeScrollView ();
			DrawLogs ();
			DrawLogDetails ();
		}



		void Update() {

			int consoleWindowLogsCount = LogEntries.GetCount ();
			int loggerWindowLogsCount = ProcessedLogs.Count;

			if(consoleWindowLogsCount != loggerWindowLogsCount) {
				if(loggerWindowLogsCount < consoleWindowLogsCount) {
					AddNewLogs (loggerWindowLogsCount);		
				} else {
					UpdateContent ();
				}
			}
		}



		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded() {
			Refresh ();
		}



		private void SetUpView() {

			if(!IsStylesInitialised) {
				InitConsoleStyles ();
			}

			HandleInputEvents ();

			LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.LogLevelLog, true);
			LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.LogLevelWarning, true);
			LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.LogLevelError, true);
			GUI.DrawTexture(new Rect(0, 0, position.width, position.height), EntryStyleBackOdd.normal.background);
		}

		private void HandleInputEvents() {
			Event e = Event.current;



			if(GUI.GetNameOfFocusedControl().Equals(SEARTCH_BAR_CONTROL_NAME)) {

				if(Event.current.type == EventType.ValidateCommand) {

					TextEditor t = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
					if (Event.current.commandName == "SelectAll"){
						t.SelectAll();
					}

					if (Event.current.commandName == "Cut"){
						t.Cut();
						searchString = string.Empty;
					}

					if (Event.current.commandName == "Copy"){
						t.Copy();
					}

					if (Event.current.commandName == "Paste"){
						t.Paste();
					}
				}
			}
		}




		string searchString = string.Empty;
		private void DrawToolbar() {

			GUILayout.BeginHorizontal (EditorStyles.toolbar); {

				bool clear = GUILayout.Button ("Clear", EditorStyles.toolbarButton);
				if(clear) {
					LogEntries.Clear ();
					UpdateContent ();
				}

				GUILayout.Space (6);
				EditorGUI.BeginChangeCheck (); {

					bool val = false;

					if(Settings.DisplayCollapse) {

						val = LogEntries.HasFlag (SA.UltimateLogger.ConsoleFlags.Collapse);
						val = GUILayout.Toggle(val, "Collapse", EditorStyles.toolbarButton);
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.Collapse, val);
					} else {
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.Collapse, false);
					}

					if(Settings.DisplayClearOnPlay) {

						val = LogEntries.HasFlag (SA.UltimateLogger.ConsoleFlags.ClearOnPlay);
						val = GUILayout.Toggle(val, "Clear on Play", EditorStyles.toolbarButton);
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.ClearOnPlay, val);

					} else {
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.ClearOnPlay, false);
					}

					if(Settings.DisplayPauseOnError) {
						val = LogEntries.HasFlag (SA.UltimateLogger.ConsoleFlags.ErrorPause);
						val = GUILayout.Toggle(val, "Error Pause", EditorStyles.toolbarButton);
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.ErrorPause, val);

					} else {
						LogEntries.SetFlag (SA.UltimateLogger.ConsoleFlags.ErrorPause, false);
					}




					if(Settings.DisplayTagsBar && !IsAllUsedTagsAreDocked) {
						if (GUILayout.Button("Tags (" +_TagsMessagesCount.Count.ToString() + ")", EditorStyles.toolbarDropDown)) {
							GenericMenu toolsMenu = new GenericMenu();

							foreach(var pair in _TagsMessagesCount) {
								var tag = Settings.GetTag (pair.Key);
								toolsMenu.AddItem(new GUIContent(tag.Name + " (" + pair.Value + ")" ), tag.Enabled, GetTagSwitchHandler(tag).Switch);
							}
							toolsMenu.ShowAsContext();
						}
					}



					GUILayout.FlexibleSpace();


					if(Settings.DisplaySeartchBar) {

						GUILayout.BeginHorizontal(ToolbarStyle); {


							GUI.SetNextControlName(SEARTCH_BAR_CONTROL_NAME);
							searchString = GUILayout.TextField(searchString, ToolbarSeachTextFieldStyle, GUILayout.MinWidth(200f));

							if (GUILayout.Button("", ToolbarSeachCancelButtonStyle)) {
								searchString = "";
								GUI.FocusControl(null);
							}

						} GUILayout.EndHorizontal();

					}

					for (int i = Settings.Tags.Count-1; i >= 0; i--) {
						var tag = Settings.Tags[i];

						if(tag.Docked) {
							var content = new GUIContent(GetMessagesCountForTag(tag.Name).ToString(), tag.Icon);
							tag.Enabled = GUILayout.Toggle(tag.Enabled, content, EditorStyles.toolbarButton);
						}

					}



				} if(EditorGUI.EndChangeCheck()) {
					UpdateContent ();
				}


			} GUILayout.EndHorizontal();

		}


		private LogInfo SelectedLogLine = null;
		private double LastMessageClickTime = 0;
		private Vector2 scrollPos = Vector2.zero;


		public void DrawLogs() {
			var collapseBadgeStyle = EditorStyles.miniButton;
			var logLineStyle = EntryStyleBackEven;

			if(Content.Count == 0) {
				SelectedLogLine = null;
				return;
			}

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height(currentScrollViewHeight));

			for(int i = 0; i < Content.Count; i++) {

				LogInfo CurrentLogLine = Content [i];
				if(CurrentLogLine.Equals(SelectedLogLine)) {
					logLineStyle = EntryStyleSelected;
				} else {
					logLineStyle = (i % 2 == 0) ? EntryStyleBackEven : EntryStyleBackOdd;
				}

				var content = GetLogLineGUIContent(CurrentLogLine);
				logLineContentSize = logLineStyle.CalcSize (content);



				bool selected;
				if(logLineContentSize.x < position.width) {
					selected = GUILayout.Button (content, logLineStyle, GUILayout.Width(position.width));
				} else {
					selected = GUILayout.Button (content, logLineStyle);
				}

				if(selected) {

					//right mouse button click
					if(Event.current.button == 1) {
						LoggerUtil.ShowLogLineMenu (CurrentLogLine);
					}

					GUI.FocusControl(null);
					if(CurrentLogLine.Equals(SelectedLogLine)) {

						if(EditorApplication.timeSinceStartup - LastMessageClickTime < DOUBLE_CLICK_TIME) {
							LastMessageClickTime = 0;

							LoggerUtil.JumpToSource (CurrentLogLine);

						} else {
							LastMessageClickTime = EditorApplication.timeSinceStartup;
						}
					} else {
						SelectedLogLine = CurrentLogLine;
						SelectedStackLine = null;
						LastMessageClickTime = EditorApplication.timeSinceStartup;
					}

					EditorGUIUtility.PingObject (CurrentLogLine.instanceID);
				}

			}


			EditorGUILayout.EndScrollView();
		}








		private LogStackLine SelectedStackLine = null;
		private double LastStackClickTime = 0;
		private Vector2 detailsScrollPos = Vector2.zero;

		private void DrawLogDetails() {
			EditorGUILayout.Space ();
			GUIStyle stackLineStyle = EntryStyleBackOdd;

			if(SelectedLogLine  != null) {

				detailsScrollPos = EditorGUILayout.BeginScrollView(detailsScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height(currentScrollViewHeight));


				EditorGUILayout.TextArea (SelectedLogLine.LogString, EntryDetailedView);
				EditorGUILayout.Space ();

				foreach(var line in SelectedLogLine.Stack) {


					if(line.Equals(SelectedStackLine)) {
						stackLineStyle = EntryStackLienSelected;
					} else {
						stackLineStyle = EntryStackLienView;
					}

					GUI.enabled = line.HasValidFilePointer;
					bool selected = GUILayout.Button (line.RawData, stackLineStyle, GUILayout.Width(position.width));
					GUI.enabled = true;

					if(selected) {


						//right mouse button click
						if(Event.current.button == 1) {
							LoggerUtil.ShowLogStackLineMenu (line);
						}


						GUI.FocusControl(null);
						if(line.Equals(SelectedStackLine)) {
							if(EditorApplication.timeSinceStartup - LastStackClickTime < DOUBLE_CLICK_TIME) {
								LastMessageClickTime = 0;
								LoggerUtil.JumpToSource (line);
							} else {
								LastStackClickTime = EditorApplication.timeSinceStartup;
							}
						} else {
							SelectedStackLine = line;
							LastStackClickTime = EditorApplication.timeSinceStartup;
						}
					}
				}

				EditorGUILayout.EndScrollView();
			}
		}




		//--------------------------------------
		// Public Methods
		//--------------------------------------



		public static void ShowConsole() {
			if (_Instance == null){
				_Instance= ScriptableObject.CreateInstance<LoggerWindow>();
				_Instance.Show ();
				_Instance.Focus();
			} else {
				_Instance.Show();
				_Instance.Focus();
			}
		}

		public static void RefreshUI() {
			if(_Instance != null) {
				InitConsoleStyles ();
				_Instance.Repaint ();
			}
		}

		public static void Refresh() {
			if(_Instance != null) {
				_Instance.UpdateContent ();
			}
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public static LoggerWindow Instance {
			get {
				return _Instance;
			}
		}


		public static LoggerSettings Settings {
			get {
				return LoggerSettings.Instance;
			}
		}

		private bool IsStylesInitialised {
			get {
				if(EntryStyleBackEven == null || EntryStyleBackOdd == null) {
					return false;
				}

				return true;
			}
		}

		private bool IsAllUsedTagsAreDocked {
			get {
				foreach(var pair in _TagsMessagesCount) {
					var tag = Settings.GetTag (pair.Key);
					if(!tag.Docked) {
						return false;
					}
				}

				return true;
			}
		}

		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private void UpdateContent() {

			Content.Clear ();
			ProcessedLogs.Clear ();
			ClearTagsMessagesCount ();

			List<LogInfo> logs = LogEntries.GetLog ();


			foreach (var log in logs) {
				AddContent (log);
			}


			//Try to preserve the selection

			if(SelectedLogLine != null) {
				int lineNumber = SelectedLogLine.LineNumber;
				if(logs.Count > lineNumber) {
					SelectedLogLine = logs [lineNumber];

					if(SelectedStackLine != null) {
						int stackLineNumber = SelectedStackLine.LineNumber;
						if(SelectedLogLine.Stack.Count > stackLineNumber) {
							if(SelectedLogLine.Stack[stackLineNumber].HasValidFilePointer) {
								SelectedStackLine = SelectedLogLine.Stack[stackLineNumber];
							} else {
								SelectedStackLine = null;
							}
						} else {
							SelectedStackLine = null;
						}
					} 
				} else {
					DisableSelection();
				}
			} else {
				DisableSelection();
			}

			Repaint ();

		}

		private void DisableSelection() {
			SelectedLogLine = null;
			SelectedStackLine = null;
		}

		private void AddNewLogs(int startIndex) {
			List<LogInfo> newLogs = LogEntries.GetLog (startIndex);

			foreach (var log in newLogs) {
				AddContent (log);
			}

			Repaint ();
		}



		private void AddContent(LogInfo log) {


			IncrementMessagesCountForTag (log.TagName);
			ProcessedLogs.Add (log);

			if(!searchString.Equals(string.Empty)) {
				if(!log.LogString.ToLower().Contains(searchString.ToLower())) {
					return;
				}
			}

			var tag = Settings.GetTag (log.TagName);
			if(tag.Enabled) {
				Content.Add (log);
				if(SelectedLogLine == null ) {
					scrollPos.y = logLineContentSize.y * Content.Count;
				}
			}

		}







		private const string TAGGED_MESSAGE_FORMAT = "[{0}]: {1}";
		private GUIContent GetLogLineGUIContent(LogInfo log) {
			var showMessage = log.LogString;

			if(!log.TagName.Equals(string.Empty) && Settings.ShowTagInMessageLine) {
				showMessage = string.Format (TAGGED_MESSAGE_FORMAT, log.TagName, log.LogString);
			}

			var content = new GUIContent(showMessage, Settings.GetTag(log.TagName).Icon);
			return content;
		}



		private float mouseStartY = -1f;
		private float oldHight = -1f;
		private void ResizeScrollView() {

			var oldColor = GUI.color;

			if(EditorGUIUtility.isProSkin) {
				GUI.color = Color.black;
			} else {
				GUI.color = Color.gray;
			}


			var lineRect = new Rect (cursorChangeRect);
			lineRect.height = 1f;
			lineRect.width = position.width;
			GUI.DrawTexture(lineRect,EditorGUIUtility.whiteTexture);
			GUI.color = oldColor; 

			EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);

			if( Event.current.type == EventType.mouseDown && cursorChangeRect.Contains(Event.current.mousePosition)){
				resize = true;
				oldHight = currentScrollViewHeight;
				mouseStartY = Event.current.mousePosition.y;
			}

			if(resize) {
				float minBorder = position.height * 0.1f;
				float newHeight =  oldHight + (Event.current.mousePosition.y - mouseStartY);
				if(newHeight > minBorder && newHeight < (position.height - minBorder) ) {
					currentScrollViewHeight = newHeight;
				}

				cursorChangeRect.Set(cursorChangeRect.x, currentScrollViewHeight + RESIZE_LINE_OFFSET ,cursorChangeRect.width, cursorChangeRect.height);
				Repaint ();
			}

			if(Event.current.type == EventType.MouseUp) {
				resize = false; 
			}

		}



		private static  void InitConsoleStyles() {


			ToolbarStyle 					= GUI.skin.FindStyle("Toolbar");
			ToolbarSeachTextFieldStyle 	= GUI.skin.FindStyle("ToolbarSeachTextField");
			ToolbarSeachCancelButtonStyle 	= GUI.skin.FindStyle("ToolbarSeachCancelButton");




			GUIStyle unityLogLineEven = null;
			GUIStyle unityLogLineOdd = null;
			GUIStyle unitySmallLogLine = null;

			foreach(var style in GUI.skin.customStyles) {
				if     (style.name=="CN EntryBackEven")  unityLogLineEven = style;
				else if(style.name=="CN EntryBackOdd")   unityLogLineOdd = style;
				else if(style.name=="CN StatusInfo")   unitySmallLogLine = style;


			}

			EntryStyleBackEven = new GUIStyle(unitySmallLogLine);



			EntryStyleBackEven.normal 	= unityLogLineEven.normal;
			EntryStyleBackEven.margin 	= new RectOffset(0,0,0,0);
			EntryStyleBackEven.padding 	= new RectOffset(Settings.logLinePadding, 0, Settings.logLinePadding, Settings.logLinePadding);
			EntryStyleBackEven.border 	= new RectOffset(0,0,0,0);
			EntryStyleBackEven.fixedHeight = 0;
			EntryStyleBackEven.alignment = TextAnchor.LowerLeft;
			EntryStyleBackEven.fontSize = Settings.fontSize;



			EntryStyleBackOdd = new GUIStyle(EntryStyleBackEven);
			EntryStyleBackOdd.normal = unityLogLineOdd.normal;

			EntryStyleSelected = new GUIStyle(EntryStyleBackEven);
			EntryStyleSelected.normal.textColor = Color.white;
			EntryStyleSelected.normal.background = Resources.Load ("console/selected_item_bg") as Texture2D;


			EntryStackLienView 	= new GUIStyle(EntryStyleBackOdd);
			EntryStackLienView.padding = new RectOffset(2, 0, 0, 0);

			EntryStackLienSelected = new GUIStyle(EntryStyleSelected);
			EntryStackLienSelected.padding = new RectOffset(2,0, 0, 0);




			EntryDetailedView = new GUIStyle(EditorStyles.textArea);
			EntryDetailedView.normal.background = null;
			EntryDetailedView.active.background = null;
			EntryDetailedView.onHover.background = null;
			EntryDetailedView.hover.background = null;
			EntryDetailedView.onFocused.background = null;
			EntryDetailedView.focused.background = null;
			EntryDetailedView.margin = new RectOffset(0,0,0,0);
			EntryDetailedView.padding = new RectOffset(2,0,0,0);
			EntryDetailedView.border = new RectOffset(0,0,0,0);
			EntryDetailedView.fontSize = Settings.fontSize;

		}



		private static void SaveSettins() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(Settings);
			#endif
		}


		//--------------------------------------
		// Tags
		//--------------------------------------

		private int GetMessagesCountForTag(string tagName) {

			if(_TagsMessagesCount.ContainsKey(tagName)) {
				return _TagsMessagesCount [tagName];
			} else {
				return 0;
			}
		}

		private void IncrementMessagesCountForTag(string tagName) {
			if(_TagsMessagesCount.ContainsKey(tagName)) {
				_TagsMessagesCount [tagName] += 1;
			} else {
				_TagsMessagesCount.Add (tagName, 1);
			}
		}


		private void ClearTagsMessagesCount() {
			_TagsMessagesCount = new Dictionary<string, int> ();
		}


		private TagSwitchHandler GetTagSwitchHandler(CustomTag tag) {
			if(_TagsSwitches.ContainsKey(tag.Name)) {
				return _TagsSwitches [tag.Name];
			} else {
				var sw = new TagSwitchHandler (tag);
				_TagsSwitches.Add (tag.Name, sw);

				return sw;
			}
		}


		//--------------------------------------
		// IHasCustomMenu
		//--------------------------------------


		void IHasCustomMenu.AddItemsToMenu(GenericMenu menu) {


			menu.AddItem(new GUIContent("Settings"), false, () => {
				LoggerMenu.ShowSettings();
			});

			menu.AddItem(new GUIContent("Copy To Clipboard"), false, () => {
				UpdateContent();

				string fullLog = string.Empty;
				foreach(var log in Content) {
					fullLog += string.Format (TAGGED_MESSAGE_FORMAT, log.TagName, log.LogString) + "\n";
				}

				EditorGUIUtility.systemCopyBuffer = fullLog;
				LoggerWindow.Instance.ShowNotification( new GUIContent("Copied To Clipboard"));

				U.Log("test me");

			});

		}



	}
}
