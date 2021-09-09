using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;
using SA.Foundation.Editor;

namespace SA.Productivity.GoogleSheets
{

    public class GD_SettingsWindow : SA_PluginSettingsWindow<GD_SettingsWindow>
    {
   
        protected override void OnAwake() {
            SetHeaderTitle(GD_Settings.PLUGIN_NAME);
            SetHeaderDescription("Gives an ability to cache data from a google spreadsheet to the unity project resources. " +
                                 "Once data is being cached the rows and cells data can be accessed using the plugin API. " +
                                 "You can use any spreadsheet formatting, only rows data is cached.");
            SetHeaderVersion(GD_Settings.FormattedVersion);
            SetDocumentationUrl(GD_Settings.DOCUMENTATION_URL);

            
            AddMenuItem("SPREADSHEETS", CreateInstance<GD_SpreadsheetsTab>()  );
            AddMenuItem("LOCALIZATION", CreateInstance<GD_LocalizationTab>());
            AddMenuItem("ABOUT", CreateInstance<IMGUIAboutTab>());
        }

		protected override void BeforeGUI() {
            EditorGUI.BeginChangeCheck();
        }


        protected override void AfterGUI() {
            if (EditorGUI.EndChangeCheck()) {
				GD_Settings.Save();
            }
        }


        public static void UpdateWorksheet(GD_DocTemplate doc, GD_WorksheetTemplate worksheet) {
#if UNITY_WEBPLAYER
                                    EditorUtility.DisplayDialog("Update Not Available", "Document cash update is not available under the Web Player platform. Since writing files is forbidden in web player platform. Switch to any other platform for updating documents cash. ", "Okay");
#else
            float currentProgress = 0f;
            EditorUtility.DisplayProgressBar("Cache " + worksheet.ListName + " Worksheet", doc.Name, currentProgress);
            GD_API.RetrievePublicSheetData(doc, worksheet, true);

            currentProgress += 100.0f;
            EditorUtility.DisplayProgressBar("Cache " + worksheet.ListName + " Worksheet", doc.Name, currentProgress);
            EditorUtility.ClearProgressBar();
#endif
        }
    }
}