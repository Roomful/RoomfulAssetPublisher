////////////////////////////////////////////////////////////////////////////////
//
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets)
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;

using SA.Foundation.Config;


namespace SA.Analytics.Google {


	public class Menu : EditorWindow {


        private const string  MENU_ROOT = SA_Config.EditorAnalyticsMenuRoot + "Google/";

		//--------------------------------------
		//  GENERAL
		//--------------------------------------


        [MenuItem(MENU_ROOT + "Edit Settings", false, SA_Config.ProductivityMenuIndex + 1)]
		public static void Edit() {
            GA_SettingsWindow.ShowTowardsInspector("Google Analytics");
		}

        [MenuItem(MENU_ROOT + "Create Analytics GameObject", false, SA_Config.ProductivityMenuIndex + 2)]
		public static void Create() {
			GameObject an = new GameObject("Google Analytics");
			an.AddComponent<GA_Manager>();
			Selection.activeObject = an;
		}

		//--------------------------------------
		//  Getting Started
		//--------------------------------------
		[MenuItem(MENU_ROOT + "Plugin Documentation/Getting Started/Setup", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAGTSetup() {
			string url = "https://unionassets.com/google-analytics-sdk/get-started-with-analytics-78";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/Getting Started/Tracking Options", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAGTTrackingOptions() {
			string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-80";
			Application.OpenURL(url);
		}

		//--------------------------------------
		//  Implementation
		//--------------------------------------

		[MenuItem(MENU_ROOT + "Plugin Documentation/Implementation/Using Basic Features Without Scripting", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAIUsingBasicFeaturesWithoutScripting() {
			string url = "https://unionassets.com/google-analytics-sdk/using-basic-features-without-scripting-265";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/Implementation/Scripting API", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAIScriptingAPI() {
			string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-82";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/Implementation/Web Player", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAIWebPlayer() {
			string url = "https://unionassets.com/google-analytics-sdk/web-player-83";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/Implementation/Campaign Measurement", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAICampaignMeasurement() {
			string url = "https://unionassets.com/google-analytics-sdk/campaign-measurement--468";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/Implementation/Advanced Fatures", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAIAdvancedFatures() {
			string url = "https://unionassets.com/google-analytics-sdk/advanced-fatures-270";
			Application.OpenURL(url);
		}

		//--------------------------------------
		//  MORE
		//--------------------------------------

		[MenuItem(MENU_ROOT + "Plugin Documentation/More/Released Apps with the plugin", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAMReleasedAppsWithThePlugin() {
			string url = "https://unionassets.com/google-analytics-sdk/released-apps-with-the-plugin-85";
			Application.OpenURL(url);
		}


		[MenuItem(MENU_ROOT + "Plugin Documentation/More/Playmaker", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAMPlaymaker() {
			string url = "https://unionassets.com/google-analytics-sdk/actions-list-84";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Plugin Documentation/More/Using Plugins with Java Script", false, SA_Config.ProductivityMenuIndex + 3)]
		public static void GAMUsingPluginsWithJavaScript() {
			string url = "https://unionassets.com/google-analytics-sdk/plugin-set-up-82#measuring-refunds";
			Application.OpenURL(url);
		}



		[MenuItem(MENU_ROOT + "Google Documentation/Measurement Protocol Developer Guide", false, SA_Config.ProductivityMenuIndex + 4)]
		public static void ProtocolDocumentation() {
			string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide";
			Application.OpenURL(url);
		}


		[MenuItem(MENU_ROOT + "Google Documentation/Measurement Protocol Parameter Reference", false, SA_Config.ProductivityMenuIndex + 4)]
		public static void ParamDocumentation() {
			string url = "https://developers.google.com/analytics/devguides/collection/protocol/v1/parameters";
			Application.OpenURL(url);
		}





		[MenuItem(MENU_ROOT + "Discussions/Unity Forum", false, SA_Config.ProductivityMenuIndex + 5)]
		public static void UnityForum() {
			string url = "http://goo.gl/B7YHzf";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Discussions/PlayMaker Forum", false, SA_Config.ProductivityMenuIndex + 5)]
		public static void PlayMakerForum() {
			string url = "http://goo.gl/0bLwcT";
			Application.OpenURL(url);
		}

		[MenuItem(MENU_ROOT + "Support", false, SA_Config.ProductivityMenuIndex + 6)]
		public static void Support() {
			string url = "http://goo.gl/QqSmBM";
			Application.OpenURL(url);
		}

	}
}
