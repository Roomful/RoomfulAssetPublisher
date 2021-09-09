using System;
using System.Collections.Generic;

namespace SA.Productivity.GoogleSheets {

	public class GD_Localization {

		/// <summary>
		/// Action is fired, when localization language chnaged
		/// </summary>
		public static event Action OnLanguageChanged = delegate {};


		//--------------------------------------
		// Initialization
		//--------------------------------------


		/// <summary>
		/// Init the localizaition API
		///
		/// Method call is optinal. You may use it on your app start
		/// otherwise localization will be initialized with fisrt localization API call
		/// </summary>
		public static void Init() {
			Client.Init();
		}



		//--------------------------------------
		// Public Methods
		//--------------------------------------

		/// <summary>
		/// <para>Set's current localization language. </para>
		/// <para> </para>
		/// LanguageChanged action will be fired once new language dictionary parsed
		/// </summary>
		public static void SetLanguage(GD_LangCode lang) {

			Client.SetLanguage(lang);
			OnLanguageChanged();
		}


		/// <summary>
		/// Returns localized string by token
		///
		/// <param name="token"> localization token</param>
		/// </summary>
		public static string GetLocalizedString(string token) {
			return Client.GetLocalizedString(token);
		}

		public static string GetLocalizedString(string token, GD_TextType textType) {
			return Client.GetLocalizedString(token, textType);
		}

		/// <summary>
		/// Returns localized string by token
		///
		/// <param name="token"> localization token</param>
		/// <param name="section"> localization section</param>
		/// </summary>
		public static string GetLocalizedString(string token, GD_LangSection section) {
			return Client.GetLocalizedString(token, section);
		}

		public static bool HasToken(string token, GD_LangSection section) {
			return Client.HasToken(token, section);
		}

		public static string GetLocalizedString(string token, GD_TextType textType, GD_LangSection section) {
			return Client.GetLocalizedString(token, textType, section);
		}

		/// <summary>
		/// Returns localized string by token
		///
		/// <param name="token"> localization token</param>
		/// <param name="section"> localization section</param>
		/// </summary>
		public static string GetLocalizedString(string token, GD_LangSection section, params object[] args) {
			string msg = Client.GetLocalizedString(token, section);
			if(args != null && args.Length > 0) {
				msg = GD_LocalizationUtil.SafeStringFormat(msg, args);
			}
			return msg;
		}

		/// <summary>
		/// Returns localized string by token
		///
		/// <param name="token"> localization token</param>
		/// <param name="section"> localization section</param>
		/// <param name="lang"> localization language code</param>
		/// </summary>
		public static string GetLocalizedString(string token, GD_LangSection section, GD_LangCode lang) {
			return Client.GetLocalizedString(token, section, lang);

		}

		/// <summary>
		/// Returns  full localization dictionary for the default section
		/// </summary>
		public static Dictionary<string, string> GetFullSectionLocalization() {
			return GetFullSectionLocalization(DefaultSection);
		}

		/// <summary>
		/// Returns  full localization dictionary for the specified section
		///
		/// <param name="section"> localization section</param>
		/// </summary>
		public static Dictionary<string, string> GetFullSectionLocalization(GD_LangSection section) {
			return Client.GetFullSectionLocalization(section);
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------


		/// <summary>
		/// Current Localization Language
		/// </summary>
		public static GD_LangCode CurrentLanguage {
			get {
				return Client.CurrentLanguage;
			}
		}


		/// <summary>
		/// DefaultLanguage Localization Language
		/// </summary>
		public static GD_LangCode DefaultLanguage {
			get {
				return (GD_LangCode)0;
			}
		}


		/// <summary>
		/// DefaultLanguage Localization Section
		/// </summary>
		public static GD_LangSection DefaultSection {
			get {
				return (GD_LangSection)0;
			}
		}




		//--------------------------------------
		// Public Section
		//--------------------------------------

		private static GD_LocalizationClient _Client = null;
		private static GD_LocalizationClient Client {
			get {
				if(_Client ==  null) {
					_Client =  new GD_LocalizationClient();
				}

				return _Client;
			}
		}


	}
}
