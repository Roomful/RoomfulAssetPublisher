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

	public static class LoggerMenu  {


	
		[MenuItem("Window/Stan's Assets/Ultimate Logger/Console", false, 400)]
		public static void ShowConsole() {
			LoggerWindow.ShowConsole ();
		}


		[MenuItem("Window/Stan's Assets/Ultimate Logger/Settings", false, 400)]
		public static void ShowSettings() {
			

			EditorWindow window = EditorWindow.GetWindow<PreferencesWindow>(true, "Ultimate Console Preferences");
			window.minSize = new Vector2(500f, 400f);
			window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
			window.position = new Rect(new Vector2(100f, 100f), window.minSize);
			window.Show();

		}

	}

}
