using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;

#if UNITY_EDITOR && !SA_DEBUG
using UnityEditor;
#endif


namespace SA.Productivity.GoogleSheets {

	internal class GD_Reader {
        public static Dictionary<string, GD_SpreadsheetData> sheets =  new Dictionary<string, GD_SpreadsheetData>();


        private static GD_DocTemplate s_requestedDoc = null;
        private static int s_worksheetIndex = -1;
        private static Action s_finishCallback = null;
        private static bool s_drawProgressBar = true;
        
        public static void RetrievePublicSheetData(GD_DocTemplate doc, bool drawProgressBar = true, Action finish = null) {
            s_requestedDoc = doc;
            s_worksheetIndex = -1;
		    s_finishCallback = finish;
            s_drawProgressBar = drawProgressBar;
            RunNextRetrieveRequest();
        }

		public static void RetrievePublicSheetData(GD_DocTemplate doc, GD_WorksheetTemplate worksheet, bool drawProgressBar = true, Action finish = null) {
			s_finishCallback = finish;
			
			StartRetrievePublicSheetData(doc, worksheet, drawProgressBar, () => {
				CompleteRetrieve();
			});
		}
		
        private static void RunNextRetrieveRequest() {
            s_worksheetIndex++;
            if (s_worksheetIndex >= s_requestedDoc.Worksheets.Count) {
                CompleteRetrieve();
                return;
            }
	        if (s_requestedDoc.Worksheets.Count > 0) {
		        GD_WorksheetTemplate worksheet = s_requestedDoc.Worksheets[s_worksheetIndex];
		        StartRetrievePublicSheetData(s_requestedDoc, worksheet, s_drawProgressBar, RunNextRetrieveRequest);
	        }
        }

        private static void CompleteRetrieve() {
            if(s_finishCallback != null) {
                s_finishCallback();
                s_finishCallback = null;
            }

			#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
			#endif
          
        }


        private  static void StartRetrievePublicSheetData(GD_DocTemplate doc, GD_WorksheetTemplate worksheet, bool drawProgressBar = true, Action finish = null) {
            #if UNITY_EDITOR && !SA_DEBUG && !UNITY_WEBPLAYER

            GD_SpreadsheetData spreadsheet;
			if (!sheets.ContainsKey(doc.Name)) {
                spreadsheet = new GD_SpreadsheetData ();
				sheets.Add(doc.Name, spreadsheet);
			} else {
				spreadsheet = sheets[doc.Name];
			}
			
			if (spreadsheet.worksheets.ContainsKey(worksheet.ListId)) {
				spreadsheet.worksheets.Remove(worksheet.ListId);
			}
			LoadSheetData (spreadsheet, worksheet.ListId, doc, drawProgressBar, () => {
                string JSON = string.Empty;
                JSON += Json.Serialize(sheets[doc.Name].worksheets);

                // Add creation DateTime stamp to file
                DateTime date = DateTime.Now;
                CultureInfo culture = CultureInfo.CurrentUICulture;
                string creationTime = date.ToString("dd MMMM, (HH:mm)\n", culture);

                //Rewrite txt file with updated data
                System.IO.File.WriteAllText(GD_Settings.GetCachePath(doc.Name), creationTime + JSON);
                doc.UpdateTime(creationTime);

                AssetDatabase.Refresh();
                
                if(finish != null) {
                    finish();
                }

            });
            #endif
		}
		
        public static void LoadSheetData(GD_SpreadsheetData spreadsheetData, int tableListId, GD_DocTemplate doc, bool drawProgressBar, Action finish) {
#if UNITY_EDITOR && !SA_DEBUG
            float p = 0f; 

            if (drawProgressBar) {
                EditorUtility.DisplayProgressBar("Caching Document", doc.Name + " worksheet: " + doc.GetWorksheetName(tableListId), 0);
            }


            string scriptUrl = GD_Settings.SCRIPT_URL_START + doc.Key + GD_Settings.SCRIPT_URL_PARAM + tableListId;
            var scriptRequest = new GD_EditorWebRequest(scriptUrl);
            scriptRequest.Send(() => {

                string spreadsheetUrl = GD_Settings.SPREADSHEET_URL_START + doc.Key + GD_Settings.SPREADSHEET_URL_END + tableListId;
                var spreadsheetReuest = new GD_EditorWebRequest(spreadsheetUrl);
                spreadsheetReuest.Send(() => {

                    if (spreadsheetReuest.Request.error != null ) {
                        Debug.LogWarning("[GoogleDataAPI] " + spreadsheetReuest.Request.error);
                        EditorUtility.ClearProgressBar();
                        return;
                    }

                    FillSpreadsheetWithData(spreadsheetData, tableListId, spreadsheetReuest.DataAsText);

                    finish();
                });

                spreadsheetReuest.OnUpdate += () => {
                    if (drawProgressBar) {
                        p += 0.01f;
                        EditorUtility.DisplayProgressBar("Caching Document", doc.Name + " worksheet: " + doc.GetWorksheetName(tableListId), p);
                    }
                };

            });
	
#endif
		}
		
		public static void RetiveSheetDataLocal(string docName) {
			string t = GetDocCache(docName);
			
			if (t != null) {
				// Offset for DateTime line
				int offset = t.IndexOf ("\n") + 1;
				string jsonStr = t.Substring (offset, t.Length - offset);
				
                GD_SpreadsheetData spreadsheet = new GD_SpreadsheetData ();
				Dictionary<string, object> worksheets = Json.Deserialize(jsonStr) as Dictionary<string, object>;
				
				Dictionary<int, Dictionary<string, string>> workSheets = new Dictionary<int, Dictionary<string, string>>();
				foreach(string key in worksheets.Keys) {
					Dictionary<string, object> lists = worksheets[key] as Dictionary<string, object>;
					
					int listId = System.Convert.ToInt32(key);
					Dictionary<string, string> cells = new Dictionary<string, string>();
					
					foreach(string cell in lists.Keys) {
						cells.Add(cell, lists[cell].ToString());
					}
					
					workSheets.Add(listId, cells);
				}
				spreadsheet.worksheets = workSheets;
				sheets.Add(docName, spreadsheet);
			} else {
                Debug.LogWarning("Such GoogleDocFile doesn't exist");
			}
		}

		private static Dictionary<string, string> DocsCacheData = new Dictionary<string, string>();
		private static string GetDocCache(string docName) {
			if(!DocsCacheData.ContainsKey(docName)) {
                TextAsset t = Resources.Load(GD_Settings.DOCS_CACHE_PATH + docName) as TextAsset;
				DocsCacheData.Add(docName, t.text);
			} 

			return DocsCacheData[docName];
		}
		
		public static void SetDocCache(string docName, int tableListId, string data) {

			//Format string returned by WWW request
			//Just delete 'google.visualization.Query.setResponse(' at the beginning
			//and ');' at the end of this string
			string json = "";
			Match match = Regex.Match(data, @"\((.+)\)", RegexOptions.IgnoreCase);
			if(match.Success) {
				json = match.Groups[1].Value;
			}
			
			Dictionary<string, object> tableEntries = Json.Deserialize (json) as Dictionary<string, object>;
			if (tableEntries == null) {
				return;
			}

			GD_SpreadsheetData spreadsheetData = new GD_SpreadsheetData();
			FillSpreadsheetWithData(spreadsheetData, tableListId, data);

			if (DocsCacheData.ContainsKey(docName)) {
				Dictionary<string, object> worksheets = Json.Deserialize(DocsCacheData[docName]) as Dictionary<string, object>;
				if (worksheets.ContainsKey(tableListId.ToString())) {
					worksheets[tableListId.ToString()] = spreadsheetData.worksheets[tableListId];
				} else {
					worksheets.Add(tableListId.ToString(), spreadsheetData.worksheets[tableListId]);
				}
				DocsCacheData[docName] = Json.Serialize(worksheets);
			} else {
				Dictionary<string, object> worksheets = new Dictionary<string, object>();
				if (worksheets.ContainsKey(tableListId.ToString())) {
					worksheets[tableListId.ToString()] = spreadsheetData.worksheets[tableListId];
				} else {
					worksheets.Add(tableListId.ToString(), spreadsheetData.worksheets[tableListId]);
				}
				DocsCacheData.Add(docName, Json.Serialize(worksheets));
			}

		}

		private static void FillSpreadsheetWithData(GD_SpreadsheetData spreadsheet, int tableListId, string data) {
			//Format string returned by WWW request
			//Just delete 'google.visualization.Query.setResponse(' at the beginning
			//and ');' at the end of this string
			string json = "";
			Match match = Regex.Match(data, @"\((.+)\)", RegexOptions.IgnoreCase);
			if(match.Success) {
				json = match.Groups[1].Value;
			}
			
			Dictionary<string, object> tableEntries = Json.Deserialize (json) as Dictionary<string, object>;
			if (tableEntries == null) {
				return;
			}
			
			int tableEntryIndex = 0;
			uint row = 1;
			uint column = 1;
			foreach (string k in tableEntries.Keys) {
				Dictionary<string, object> cells = tableEntries[k] as Dictionary<string, object>;
				if (cells != null) {
					
					foreach (string key in cells.Keys) {
						if (key.Equals("cols")) {
							// You can get access for table columns here						
						} else if (key.Equals("rows")) {
							List<object> rows = cells[key] as List<object>;
							
							foreach(object rk in rows) {
								Dictionary<string, object> rcells = rk as Dictionary<string, object>;
								
								foreach (string crk in rcells.Keys) {
									List<object> d = rcells[crk] as List<object>;
									
									foreach(object res in d) {
										
										Dictionary<string, object> resource = res as Dictionary<string, object>;
										if (resource != null) {
											foreach(string rkey in resource.Keys) {
												if (rkey == "v") {
													if (resource[rkey] == null) {
														spreadsheet.SetData(tableListId, column, row, "null");
													} else {
														spreadsheet.SetData(tableListId, column, row, resource[rkey].ToString());
													}
													row++;
												}
											}
										} else {
											spreadsheet.SetData(tableListId, column, row, "null");
											row++;
										}
									}
								}
								row = 1;
								column++;
							}
						}
					}
					tableEntryIndex++;
				}
			}
		}
		
	}

}
