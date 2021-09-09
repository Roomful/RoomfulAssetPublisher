using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace SA.Productivity.GoogleSheets {

	[Serializable]
	public class GD_DocTemplate {

		public string Key;
        public string Name = string.Empty;

		public string DocName = string.Empty;
        public int DocIndex = 0;
        public int SelectedWorksheet = 0;
	


        private string m_creationTime = string.Empty;
        [SerializeField] List<GD_WorksheetTemplate> m_worksheets = new List<GD_WorksheetTemplate>();


		public void InitDoc() {
			int add = 1;
			// Select unique Document name
			do {
				Name = "NewDocument" + (GD_Settings.Instance.Documents.Count + add);
				add++;
			} while (GD_Settings.Instance.GetDocByName(Name) != null);

			// Add table with gid=0 by default
            m_worksheets.Add(new GD_WorksheetTemplate("Sheet1", 0));
		}

		public void AddNewWorksheet() {
            m_worksheets.Add(new GD_WorksheetTemplate("Sheet" + (m_worksheets.Count + 1), m_worksheets.Count));
		}

        public void RemoveWorksheet(GD_WorksheetTemplate list) {
			m_worksheets.Remove(list);
		}

		public int GetWorksheetId(string name) {
			int id = 0;
			foreach (GD_WorksheetTemplate list in m_worksheets) {
				if (list.ListName == name) {
					return list.ListId;
				}
			}

			return id;
		}

		public string GetWorksheetName(int id) {
			string name = id.ToString();
            foreach (GD_WorksheetTemplate list in m_worksheets) {
				if (list.ListId == id) {
					return list.ListName;
				}
			}

			return name;
		}

		public bool WorksheetExist(string name) {
			int same = 0;
            foreach (GD_WorksheetTemplate list in m_worksheets) {
				if (list.ListName == name) {
					same++;
				}
			}

			return same > 1 ? true : false;
		}

		public bool WorksheetExist(int id) {
			int same = 0;
            foreach (GD_WorksheetTemplate list in m_worksheets) {
				if (list.ListId == id) {
					same++;
				}
			}

			return same > 1 ? true : false;
		}

		public void UpdateTime(string timeStr) {
			m_creationTime = timeStr;
		}

		public string[] GetWorksheetNames() {
			List<string> names = new List<string>();
            foreach (GD_WorksheetTemplate w in m_worksheets) {
				names.Add(w.ListName);
			}

			return names.ToArray();
		}

        public List<GD_WorksheetTemplate> Worksheets {
			get { return m_worksheets; }
		}

        public bool HasCache {
            get {
                return File.Exists(GD_Settings.GetCachePath(Name));
            }
        }

		public string CreationTime {
			get {
				if (HasCache) {
					StreamReader reader = new StreamReader(new FileStream(GD_Settings.GetCachePath(Name), FileMode.Open));
					m_creationTime = reader.ReadLine();
					reader.Dispose();
				} else {
					m_creationTime = string.Empty;
				}

				return m_creationTime;
			}
		}
	}
}
