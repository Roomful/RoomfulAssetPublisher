using UnityEngine;
using System;
using System.Collections.Generic;

namespace SA.Productivity.GoogleSheets {

	

	public static class GD_API  {

        public const string NULL_VALUE = "null";


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public static T GetValue<T> (string docName, int row, int col, int workSheetNumber = 0) {
			T value = default(T);

			if (!GD_Reader.sheets.ContainsKey(docName)) {
				GD_Reader.RetiveSheetDataLocal(docName);
			}
			
			if (GD_Reader.sheets.ContainsKey(docName)) {
				GD_SpreadsheetData spreadsheet = GD_Reader.sheets[docName];
				if (spreadsheet != null) {
					string data = spreadsheet.GetData (workSheetNumber, row, col);
                    if(data.Equals(NULL_VALUE)) {
                        return default(T);
                    }

					try {
						value = (T)Convert.ChangeType (data, typeof(T));
					} catch (FormatException){
						return default(T);
					}
				}
			}

			return value;
		}

		public static T GetValue<T> (string docName, GD_Cell cell, int workSheetNumber = 0) {
			return GetValue<T>(docName, cell.row, cell.col, workSheetNumber);
		}



		public static List<T> GetList<T>(string docName, GD_CellRange range, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			if (doc != null) {
				return  GetList<T> (docName, range, doc.GetWorksheetId(workSheetName));
			} else {
				return new List<T>();
			}
		}

        public static List<GD_Cell> GetCellsList(string docName, GD_CellRange range, int workSheetNumber = 0) {
            List<GD_Cell> result = new List<GD_Cell>();

            string data = string.Empty;

            GD_Cell cell = new GD_Cell(range.StartCell.col, range.StartCell.row);

            if (range.Direction == GD_RanageDirection.Row) {
                int lenght = 1;
                for (; ; ) {
                    GD_Cell valueCell = cell.Copy();
                    data = GetValue<string>(docName, valueCell, workSheetNumber);
                    if(string.IsNullOrEmpty(data)) {
                        if (range.UseLinebreak && lenght >= range.LineLength) {
                            cell.col = range.StartCell.col;
                            cell.row++;
                            lenght = 1;
                        } else {
                            return result;
                        }
                    } else {
                        if (range.UseLinebreak && lenght >= range.LineLength) {
                            cell.col = range.StartCell.col;
                            cell.row++;
                            lenght = 1;
                        } else {
                            lenght++;
                            cell.col++;
                        }
                        result.Add(valueCell);
                    }
                }
            } else if (range.Direction == GD_RanageDirection.Collumn) {
                int lenght = 1;
                for (; ; ) {
                    GD_Cell valueCell = cell.Copy();
                    data = GetValue<string>(docName, valueCell, workSheetNumber);

                    if (string.IsNullOrEmpty(data)) {
                        if (range.UseLinebreak && lenght >= range.LineLength) {
                            cell.row = range.StartCell.row;
                            cell.col++;
                            lenght = 1;
                        } else {
                            return result;
                        }
                    } else {
                        if (range.UseLinebreak && lenght >= range.LineLength) {
                            cell.row = range.StartCell.row;
                            cell.col++;
                            lenght = 1;
                        } else {
                            lenght++;
                            cell.row++;
                        }

                        result.Add(valueCell);
                    }
                }
            }

            return result;
        }

  
		public static List<T> GetList<T>(string docName, GD_CellRange range, int workSheetNumber = 0) {


            List<T> result = new List<T>();

            List<GD_Cell> cells = GetCellsList(docName, range, workSheetNumber);
            foreach(var cell in cells) {
                T value = GetValue<T>(docName, cell, workSheetNumber);
                result.Add(value);
            }

            return result;
        }

		public static T[] GetArray<T>(string docName, GD_CellRange range, int workSheetNumber = 0) {
			return GetList<T> (docName, range, workSheetNumber).ToArray ();
		}

		public static T[] GetArray<T>(string docName, GD_CellRange range, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			List<T> result = new List<T> ();
			if (doc != null) {
				result = GetList<T> (docName, range, doc.GetWorksheetId(workSheetName));
			}
			return result.ToArray();
		}



		public static Dictionary<K, V> GetDictionary<K, V>(string docName, GD_CellDictionaryRange dictionaryRange, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);

			if (doc != null) {
				return  GetDictionary<K, V> (docName, dictionaryRange, doc.GetWorksheetId(workSheetName));
			} else {
				return new Dictionary<K, V>();
			}
		}

		public static Dictionary<K, V> GetDictionary<K, V>(string docName, GD_CellDictionaryRange dictionaryRange, int workSheetId = 0) {

            Dictionary<K, V> result = new Dictionary<K, V> ();
            List<GD_Cell> keyCells = GetCellsList(docName, dictionaryRange.CellRange, workSheetId);
            foreach (var keyCell in keyCells) {
                var valueCell = dictionaryRange.GetValueCellForKey(keyCell);
 
                K key = GetValue<K>(docName, keyCell, workSheetId);
                V value = GetValue<V>(docName, valueCell, workSheetId);
                if(result.ContainsKey(key)) {
                    var doc = GD_Settings.Instance.GetDocByName(docName);
                    Debug.LogWarning("Duplicated key '" + key.ToString() + "'  skipped when parsing Dictionary. " +
                        "Duplicate occured at cell: " + keyCell.key + " doc: " + docName + " worksheet: " + doc.GetWorksheetName(workSheetId));
                    continue;
                }
                result.Add(key, value);
            }
            return result;

		}




		public static bool IsValueOfType<T>(string docName, int row, int col, int workSheetNumber = 0) {

			if (!GD_Reader.sheets.ContainsKey(docName)) {
				GD_Reader.RetiveSheetDataLocal(docName);
			}
			
			if (GD_Reader.sheets.ContainsKey(docName)) {
				GD_SpreadsheetData spreadsheet = GD_Reader.sheets[docName];
				if (spreadsheet != null) {
					string data = spreadsheet.GetData (workSheetNumber, row, col);
					
					try {
						Convert.ChangeType (data, typeof(T));
						return true;
					} catch (FormatException){
						return false;
					}
				}
			}
			return false;

		}


		public static bool IsValueOfType<T>(string docName, GD_Cell cell, int workSheetNumber = 0) {
			return IsValueOfType<T>(docName, cell.row, cell.col, workSheetNumber);
		}

		public static GD_Cell FindCellByContent(string docName, object content, int workSheetNumber = 0) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				// Begin search from cell A1
				GD_Cell cell = new GD_Cell (1, 1);
				string value = GetValue<string>(docName, cell, workSheetNumber);
				int nEmpty = 0;
				while (nEmpty < 2) {
					
					if (value.Equals(content.ToString())){
						return cell;
					}

					if (value.Equals(string.Empty)) {
						cell.col = 1;
						cell.row++;
						nEmpty++;
					} else {
						cell.col++;
						nEmpty = 0;
					}

					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}
			
			return null;
		}


		public static GD_Cell FindCellByContent(string docName, object content, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellByContent(docName, content, doc.GetWorksheetId(workSheetName));
			}
			
			return null;
		}


		public static GD_Cell FindCellByContent(string docName, string column, object content, int workSheetNumber = 0) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);

			if (doc != null) {
				// Begin search from cell with row = 1
				GD_Cell cell = new GD_Cell (column + "1");
				string value = GetValue<string>(docName, cell, workSheetNumber);
				while (!value.Equals(string.Empty)) {

					if (value.Equals(content.ToString())){
						return cell;
					}

					cell.row++;
					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}

			return null;
		}


		public static GD_Cell FindCellByContent(string docName, string column, object content, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellByContent(docName, column, content, doc.GetWorksheetId(workSheetName));
			}
			
			return null;
		}
		
		
		public static GD_Cell FindCellByContent(string docName, int row, object content, int workSheetNumber = 0) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				// Begin search from cell with collumn = 1
				GD_Cell cell = new GD_Cell (1, row);
				string value = GetValue<string>(docName, cell, workSheetNumber);
				while (!value.Equals(string.Empty)) {
					
					if (value.Equals(content.ToString())){
						return cell;
					}
					
					cell.col++;
					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}
			
			return null;
		}


		public static GD_Cell FindCellByContent(string docName, int row, object content, string workSheetName) {
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellByContent(docName, row, content, doc.GetWorksheetId(workSheetName));
			}
			
			return null;
		}


		public static GD_Cell[] FindCellsByContent(string docName, object content, int workSheetNumber = 0) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				// Begin search from cell A1
				GD_Cell cell = new GD_Cell (1, 1);
				string value = GetValue<string>(docName, cell, workSheetNumber);
				int nEmpty = 0;
				while (nEmpty < 2) {
					
					if (value.Equals(content.ToString())){
						cells.Add(new GD_Cell(cell.col, cell.row));
					}
					
					if (value.Equals(string.Empty)) {
						cell.col = 1;
						cell.row++;
						nEmpty++;
					} else {
						cell.col++;
						nEmpty = 0;
					}
					
					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}

			return cells.ToArray();
		}


		public static GD_Cell[] FindCellsByContent(string docName, object content, string workSheetName) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellsByContent(docName, content, doc.GetWorksheetId(workSheetName));
			}
			
			return cells.ToArray();
		}


		public static GD_Cell[] FindCellsByContent(string docName, string column, object content, int workSheetNumber = 0) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				// Begin search from cell with row = 1
				GD_Cell cell = new GD_Cell (column + "1");
				string value = GetValue<string>(docName, cell, workSheetNumber);
				while (!value.Equals(string.Empty)) {
					
					if (value.Equals(content.ToString())){
						cells.Add(new GD_Cell(cell.key));
					}
					
					cell.row++;
					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}

			return cells.ToArray ();
		}


		public static GD_Cell[] FindCellsByContent(string docName, string column, object content, string workSheetName) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellsByContent(docName, column, content, doc.GetWorksheetId(workSheetName));
			}
			
			return cells.ToArray();
		}


		public static GD_Cell[] FindCellsByContent(string docName, int row, object content, int workSheetNumber = 0) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				// Begin search from cell with collumn = 1
				GD_Cell cell = new GD_Cell (1, row);
				string value = GetValue<string>(docName, cell, workSheetNumber);
				while (!value.Equals(string.Empty)) {
					
					if (value.Equals(content.ToString())){
						cells.Add(new GD_Cell(cell.key));
					}
					
					cell.col++;
					value = GetValue<string>(docName, cell, workSheetNumber);
				}
			}
			
			return cells.ToArray();
			
		}

		
		public static GD_Cell[] FindCellsByContent(string docName, int row, object content, string workSheetName) {
			List<GD_Cell> cells = new List<GD_Cell> ();
			GD_DocTemplate doc = GD_Settings.Instance.GetDocByName (docName);
			
			if (doc != null) {
				return FindCellsByContent(docName, row, content, doc.GetWorksheetId(workSheetName));
			}
			
			return cells.ToArray();
		}



		//--------------------------------------
		// Utils
		//--------------------------------------


		public static void RetrievePublicSheetData(GD_DocTemplate doc, bool drawProgressBar = true, Action finish = null) {
			GD_Reader.RetrievePublicSheetData(doc, drawProgressBar, finish);
		}

		public static void RetrievePublicSheetData(GD_DocTemplate doc, GD_WorksheetTemplate worksheet, bool drawProgressBar = true, Action finish = null) {
			GD_Reader.RetrievePublicSheetData(doc, worksheet, drawProgressBar, finish);
		}
			
	}

}
