////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SA.UltimateLogger {


	public class LoggerSettings : ScriptableObject {

		//--------------------------------------
		// Const
		//--------------------------------------

		public const string VERSION_NUMBER = "1.0.0";

		public const string MESSAGE_TAG_NAME = "message";
		public const string WARNING_TAG_NAME = "warning";
		public const string ERROR_TAG_NAME = "error";


		public const string SETTINGS_LOCATION = "Plugins/StansAssets/Support/Settings/Editor/Resources/";



		//--------------------------------------
		// Console Window
		//--------------------------------------


		public List<string> IgnoredWrapperClasses = new List<string>();



		//--------------------------------------
		// Editor Window
		//--------------------------------------


		//PreferencesWindow General
		public int fontSize = 11;
		public int logLinePadding = 2;
		public bool ShowTagInMessageLine = false;


		//PreferencesWindow ToolBar

		public bool DisplayCollapse 	= true; 
		public bool DisplayClearOnPlay 	= true; 
		public bool DisplayPauseOnError = true; 
		public bool DisplaySeartchBar 	= true; 
		public bool DisplayTagsBar 		= true; 

		//PreferencesWindow Tags
		public List<CustomTag> Tags = new List<CustomTag> ();



		private const string UL_SettingsAssetName = "LoggerSettings";
		private const string UL_SettingsAssetExtension = ".asset";

		private static LoggerSettings instance = null;
		public static LoggerSettings Instance {
			
			get {
				if (instance == null) {
					instance = Resources.Load(UL_SettingsAssetName) as LoggerSettings;

					if (instance == null) {

						// If not found, autocreate the asset object.
						instance = CreateInstance<LoggerSettings>();
						#if UNITY_EDITOR


						SA.UltimateLogger.EditorUtils.CreateFolder(SETTINGS_LOCATION);
						string fullPath = Path.Combine(Path.Combine("Assets", SETTINGS_LOCATION),
							UL_SettingsAssetName + UL_SettingsAssetExtension
						);

						AssetDatabase.CreateAsset(instance, fullPath);

						instance.Init();
						#endif
					}
				}
				return instance;
			}
		}


		public void Init() {
			InitDefaultTags ();
		}

		public void InitDefaultTags() {

			#if UNITY_EDITOR

			if(Tags.Count == 0) {


				var tag = new CustomTag ();
				tag.Name = ERROR_TAG_NAME;
				tag.Icon = EditorGUIUtility.FindTexture( "d_console.erroricon.sml" );
				tag.Docked = true;
				tag.Enabled = true;
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = WARNING_TAG_NAME;
				tag.Icon = EditorGUIUtility.FindTexture( "d_console.warnicon.sml" );
				tag.Docked = true;
				tag.Enabled = true;
				Tags.Add (tag);


				tag = new CustomTag ();
				tag.Name = MESSAGE_TAG_NAME;
				tag.Icon = EditorGUIUtility.FindTexture( "d_console.infoicon.sml" );
				tag.Docked = true;
				tag.Enabled = true;
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "network";
				tag.Icon =  Resources.Load ("icons/netwrok") as Texture2D; 
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "gameplay";
				tag.Icon =  Resources.Load ("icons/gameplay") as Texture2D; 
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "service";
				tag.Icon =  Resources.Load ("icons/service") as Texture2D; 
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "cloud";
				tag.Icon =  Resources.Load ("icons/cloud") as Texture2D; 
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "in";
				tag.Icon =  Resources.Load ("icons/down") as Texture2D; 
				Tags.Add (tag);

				tag = new CustomTag ();
				tag.Name = "out";
				tag.Icon =  Resources.Load ("icons/up") as Texture2D; 
				Tags.Add (tag);

				Save ();
			}
			#endif

		}
			

		public CustomTag GetTag(string tagName) {
			InitDefaultTags ();

			foreach(var tag in Tags) {
				if(tag.Name.Equals(tagName)){
					return tag;
				}
			}

			var newTag = new CustomTag ();
			newTag.Name = tagName;
			newTag.Icon =  Resources.Load ("icons/tag") as Texture2D; 
			Tags.Add (newTag);

			Save ();

			return newTag;
		}



		public void Save() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
			#endif
		}

	}
}
