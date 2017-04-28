////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace SA.UltimateLogger {


	[System.Serializable]
	public class CustomTag  {

		public string Name;
		public Texture2D Icon;

		public bool Docked = false;
		public bool Enabled = true;


		public GUIContent DisaplyContent {

			get {
				GUIContent content = new GUIContent ();
				if(Icon != null) {
					content.image = Icon;
				}

				content.text = Name;
				return content;
			}
		}
	}
}
