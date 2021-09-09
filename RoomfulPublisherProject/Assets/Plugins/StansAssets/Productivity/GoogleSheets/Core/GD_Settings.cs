using UnityEngine;
using System.IO;
using System.Collections.Generic;

using SA.Foundation.Config;
using SA.Foundation.Patterns;
using SA.Foundation.Utility;
using SA.Foundation.UtilitiesEditor;


namespace SA.Productivity.GoogleSheets {

    public class GD_Settings : SA_ScriptableSingleton<GD_Settings> {


        //--------------------------------------
        // Constants
        //--------------------------------------


        public const string PLUGIN_NAME = "Google Sheets";
        public const string PLUGIN_FOLDER = SA_Config.StansAssetsProductivityPluginsPath + "GoogleSheets/";


        public const string DOCUMENTATION_URL = "https://unionassets.com/google-doc-connector/manual";
		public static string GOOGLEDOC_URL_START = "https://docs.google.com/spreadsheets/d/";
		public static string GOOGLEDOC_URL_END = "#gid=";

		public static string SPREADSHEET_URL_START = "https://spreadsheets.google.com/tq?&tq=&key=";
		public static string SPREADSHEET_URL_END = "&gid=";

		public static string SCRIPT_URL_START = "https://script.google.com/macros/s/AKfycbwrDJWLwK7BuA0KCOap3uavVFdPpoqGoffJu9zxigOO-kuAUFw/exec?key=";
		public static string SCRIPT_URL_PARAM = "&sheetid=";

        public static string DOCS_CACHE_PATH 	= "GoogleDocumentsCache/";




        //--------------------------------------
        // Properties
        //--------------------------------------


        public List<GD_DocTemplate> Documents = new List<GD_DocTemplate>();


        //--------------------------------------
        // Localization Settings
        //--------------------------------------

        public string LocalizationDocKey = string.Empty;

	    public static string LOCALIZATION_LOG_PREFIX = "GDC Localization: ";

        public const string PLUGIN_FOLDER_PATH = SA_Config.StansAssetsProductivityPluginsPath + "GoogleSheets/";
        public const string LOCALIZATION_ENUMS_PATH = PLUGIN_FOLDER_PATH + "Modules/Localization/Enums/";


        public GD_DocTemplate LocalizationDoc {
            get {
                return GetDocByKey(LocalizationDocKey);
            }
        }


        //--------------------------------------
        // Publi Methods
        //--------------------------------------


		public static string GetCachePath() {
            //We only writing on disc inside the unity Editor
            var relativeCachePath = SA_Config.StansAssetsCachePath + DOCS_CACHE_PATH;

            if (!SA_AssetDatabase.IsDirectoryExists(relativeCachePath)) {
                SA_AssetDatabase.CreateFolder(relativeCachePath);
            }
            return SA_PathUtil.ConvertRelativeToAbsolutePath(relativeCachePath) + SA_PathUtil.FOLDER_SEPARATOR;
		}

		public static string GetCachePath(string docName) {
			return GetCachePath() + docName + ".txt";
		}
        public static string GetRelativeCachePath(string docName) {
            var relativeCachePath = SA_Config.StansAssetsCachePath + DOCS_CACHE_PATH;
            return relativeCachePath + docName + ".txt";
        }


		public void AddDoc(GD_DocTemplate tpl) {
			Documents.Add(tpl);
		}
		public void RemoveDoc(GD_DocTemplate tpl) {
			Documents.Remove(tpl);
            SA_AssetDatabase.DeleteAsset(GetRelativeCachePath(tpl.Name));
		}



		public int GetDocIndexByName(string name) {
			foreach(GD_DocTemplate d in Documents) {
	            if (d != null) {
	                if (name.Equals(d.Name)) {
	                    int index = Documents.IndexOf(d) + 1;
	                    return index;
	                }
	            }
			}

			return 0;
		}


			public int GetDocIndexByKey(string key) {
				foreach(GD_DocTemplate d in Documents) {
					if (d != null) {
						if (key.Equals(d.Key)) {
							int index = Documents.IndexOf(d) + 1;
							return index;
						}
					}
				}

				return 0;
			}




		public GD_DocTemplate GetDocByName(string name) {

			foreach(GD_DocTemplate d in Documents) {
	            if (d != null) {
	                if(name.Equals(d.Name)) {
					    return d;
				    }
	            }
			}

			return null;
		}

		public GD_DocTemplate GetDocByKey(string key) {
			foreach(GD_DocTemplate d in Documents) {
				if (d != null) {
					if(key.Equals(d.Key)) {
						return d;
					}
				}
			}

			return null;
		}


		public GD_DocTemplate GetDocByIndex(int index) {

			if(index == 0) {
				return null;
			}

			index--;


			if(index >= Documents.Count) {
				return null;
			} else {
				return Documents[index];
			}
		}



		public string[] GetDocNames() {
			List<string> names =  new List<string>();
			names.Add("None");
			foreach(GD_DocTemplate d in Documents) {
	            if (d != null) {
	                names.Add(d.Name);
	            }
			}

			return names.ToArray();
		}


        public static void OpenDocURL(GD_DocTemplate doc, GD_WorksheetTemplate worksheet = null) {
			int id = 0;
			if(worksheet != null) {
				id = worksheet.ListId;
			}
			Application.OpenURL(GD_Settings.GOOGLEDOC_URL_START + doc.Key + GD_Settings.GOOGLEDOC_URL_END + id.ToString());
		}

	    protected override string BasePath {
		    get { return PLUGIN_FOLDER; }
	    }

	    public override string PluginName { get; }
	    public override string DocumentationURL { get; }
	    public override string SettingsUIMenuItem { get; }
    }

}
