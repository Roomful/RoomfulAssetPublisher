using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SA.Productivity.GoogleSheets {

    internal class GD_SpreadsheetData  {

        private  Dictionary<int, Dictionary<string, string>> m_worksheets =  new Dictionary<int, Dictionary<string, string>>() ;

		public void SetData(int worksheetIndex, uint row, uint col, string data) {

			Dictionary<string, string> cells = null;
			if(m_worksheets.ContainsKey(worksheetIndex)) {
				cells = m_worksheets[worksheetIndex];
			} else {
				cells =  new Dictionary<string, string>();
				m_worksheets.Add(worksheetIndex, cells);
			}

			string cellKey = row.ToString() + ":" + col.ToString();
			if(cells.ContainsKey(cellKey)) {
				cells[cellKey] = data;
			} else {
				cells.Add(cellKey, data);
			}

		}

		public string GetData(int worksheetIndex, int row, int col) {
			Dictionary<string, string> cells = null;
			if(m_worksheets.ContainsKey(worksheetIndex)) {
				cells = m_worksheets[worksheetIndex];

				string cellKey = row.ToString() + ":" + col.ToString();
				if(cells.ContainsKey(cellKey)) {
					return cells[cellKey];
				}
			}

			return string.Empty;
		}

		public Dictionary<int, Dictionary<string, string>> worksheets {
			get {
				return m_worksheets;
			}
			set {
				m_worksheets = value;
			}
		}
	}

}
