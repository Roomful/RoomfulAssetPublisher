using UnityEngine;
using System.Collections.Generic;
using System.Globalization;

namespace SA.Productivity.GoogleSheets
{
    internal class GD_LocalizationClient
    {
        private GD_LangCode m_langCode;
        private Dictionary<string, Dictionary<string, string>> m_sheetDictionary = new Dictionary<string, Dictionary<string, string>>();

        //--------------------------------------
        // Initialization
        //--------------------------------------

        public GD_LocalizationClient() {
            try {
                SetLanguage(GD_LocalizationPrefs.CurrentLanguage);
            }
            catch (System.Exception ex) {
                Debug.LogWarning(GD_Settings.LOCALIZATION_LOG_PREFIX + "parcing failed: " + ex.Message);
                SetLanguage(GD_Localization.DefaultLanguage);
            }
        }

        public void Init() { }

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SetLanguage(GD_LangCode lang) {
            m_langCode = lang;

            FillLocalizationDictionary(m_langCode);
            GD_LocalizationPrefs.SaveLocalizationChose(lang);
        }

        public string GetLocalizedString(string token, GD_TextType _type, GD_LangSection section = 0) {
            string text = GetLocalizedString(token, section);

            switch (_type) {
                case GD_TextType.ToLower:
                    text = text.ToLowerInvariant();
                    break;
                case GD_TextType.ToUppper:
                    text = text.ToUpperInvariant();
                    break;
                case GD_TextType.WithCapital:
                    if (text.Length > 0) {
                        text = GD_LocalizationUtil.Concat(char.ToUpper(text[0]), text.Substring(1).ToLowerInvariant());
                    }

                    break;
                case GD_TextType.EachWithCapital:
#if !UNITY_WSA
                    text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
#else
						if (text.Length > 0) {
							text = StringUtil.Concat(char.ToUpper(text[0]), text.Substring(1).ToLowerInvariant());
						}
#endif
                    break;
            }

            return text;
        }

        public string GetLocalizedString(string token) {
            return GetLocalizedString(token, GD_Localization.DefaultSection, m_langCode);
        }

        public string GetLocalizedString(string token, GD_LangSection section) {
            return GetLocalizedString(token, section, m_langCode);
        }

        public bool HasToken(string token, GD_LangSection section) {
            if (!m_sheetDictionary.ContainsKey(section.ToString())) {
                return false;
            }

            if (!m_sheetDictionary[section.ToString()].ContainsKey(token)) {
                return false;
            }

            return true;
        }

        public string GetLocalizedString(string token, GD_LangSection section, GD_LangCode language) {
            if (!language.Equals(CurrentLanguage)) {
                return GetUparcedTokenValue(token, section, language);
            }

            if (!m_sheetDictionary.ContainsKey(section.ToString())) {
                Debug.LogWarning(GD_Settings.LOCALIZATION_LOG_PREFIX + section.ToString() + " Section Not Found");
                return token + "_key_not_found";
            }
            else {
                if (!m_sheetDictionary[section.ToString()].ContainsKey(token)) {
                    Debug.LogWarning(GD_Settings.LOCALIZATION_LOG_PREFIX + token + " Token Not Found under the " + section.ToString() + " Section");
                    return token + "_key_not_found";
                }
            }

            return m_sheetDictionary[section.ToString()][token];
        }

        public Dictionary<string, string> GetFullSectionLocalization(GD_LangSection section) {
            string key = section.ToString();
            if (m_sheetDictionary.ContainsKey(key)) {
                return m_sheetDictionary[key];
            }

            return new Dictionary<string, string>();
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public GD_LangCode CurrentLanguage {
            get { return m_langCode; }
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        public void FillLocalizationDictionary(GD_LangCode lang) {
            if (GD_Settings.Instance.LocalizationDoc == null) {
                Debug.LogWarning(GD_Settings.LOCALIZATION_LOG_PREFIX + "No Localization doc found, open localization settings.");
                return;
            }

            string DocName = GD_Settings.Instance.LocalizationDoc.Name;

            m_sheetDictionary = new Dictionary<string, Dictionary<string, string>>();
            foreach (GD_LangSection section in System.Enum.GetValues(typeof(GD_LangSection))) {
                string WorkSheetName = section.ToString();

                GD_CellRange KeysRange = new GD_CellRange("A2", GD_RanageDirection.Collumn);
                GD_CellDictionaryRange ValuesRange = new GD_CellDictionaryRange(KeysRange, (int) lang + 1, 0);

                Dictionary<string, string> dict = GD_API.GetDictionary<string, string>(DocName, ValuesRange, WorkSheetName);
                Dictionary<string, string> localizationDict = new Dictionary<string, string>();

                foreach (KeyValuePair<string, string> pair in dict) {
                    string key = pair.Key.Trim();
                    string val = pair.Value;
                    if (string.IsNullOrEmpty(val)) {
                        Debug.LogWarning("Value us missing for " + key + " key. Lang: " + lang + " Section: " + WorkSheetName);
                        val = string.Empty;
                    }

                    localizationDict.Add(key, val);
                }

                m_sheetDictionary.Add(WorkSheetName, localizationDict);
            }
        }

        private string GetUparcedTokenValue(string token, GD_LangSection section, GD_LangCode lang) {
            string DocName = GD_Settings.Instance.LocalizationDoc.Name;
            string WorkSheetName = section.ToString();
            GD_CellRange KeysRange = new GD_CellRange("A2", GD_RanageDirection.Collumn);
            GD_CellDictionaryRange ValuesRange = new GD_CellDictionaryRange(KeysRange, (int) lang + 1, 0);

            Dictionary<string, string> dict = GD_API.GetDictionary<string, string>(DocName, ValuesRange, WorkSheetName);

            foreach (KeyValuePair<string, string> pair in dict) {
                if (pair.Key.Trim().Equals(token)) {
                    return pair.Value.Trim();
                }
            }

            return token;
        }
    }
}
