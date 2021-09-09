using UnityEngine;


namespace SA.Productivity.GoogleSheets {

    internal class GD_LocalizationPrefs {

		private const string PREFS_LANG_KEY = "SA_PREFS_LANG_KEY";
		 

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public static void SaveLocalizationChose(GD_LangCode lang) {
			PlayerPrefs.SetInt(PREFS_LANG_KEY, (int)lang);
		}

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public static GD_LangCode CurrentLanguage {
			get {
				if(HasSavedLocalization) {
					return (GD_LangCode) PlayerPrefs.GetInt(PREFS_LANG_KEY);
				} else {
					return GD_Localization.DefaultLanguage;
				}
			}
		}


		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private static bool HasSavedLocalization {
			get {
				return PlayerPrefs.HasKey(PREFS_LANG_KEY);
			}
		}

	}
}
