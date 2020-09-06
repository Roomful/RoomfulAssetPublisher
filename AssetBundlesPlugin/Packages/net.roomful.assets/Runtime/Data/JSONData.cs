using System;
using UnityEngine;
using System.Collections.Generic;
using SA.Foundation.Time;
using StansAssets.Foundation;

namespace net.roomful.assets {
	
	public class JSONData  {

		private Dictionary<string, object> _Data = null;
		private string _RawData = string.Empty;

		private bool _IsValid = false;



		public JSONData(string data) {
			try {
				_Data =   Json.Deserialize(data) as Dictionary<string, object>;
				_RawData = data;
				_IsValid = true;

			} catch(System.Exception ex) {
				Debug.LogError (ex.Message);
				Debug.LogError ("Can't parse JSONData out of: " + data);
			}
		}

		public JSONData(object data) {
			try {
				_Data = (Dictionary<string, object>) data;
				_RawData = Json.Serialize(data);
				_IsValid = true;

			} catch(System.Exception ex) {
				Debug.LogError (ex.Message);
				Debug.LogError ("Can't parse JSONData out of: " + data);
			}
		}



		public bool HasValue(params string[] keys) { 

			Dictionary<string, object> dict = _Data;
			for(int i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}


			string valueKey = keys[keys.Length - 1];

			if(dict.ContainsKey(valueKey)) {
				return dict[valueKey] != null;
			} else {
				return false;
			}
			
		}

		public T GetValue<T>(params string[] keys) {

			T value = default(T);
			Dictionary<string, object> dict = _Data;
			for(int i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}


			string valueKey = keys[keys.Length - 1];
			object data = dict[valueKey];

			if(typeof (T) == typeof( DateTime)) {
				
				string dateString = Convert.ToString(data);
				DateTime date;

				bool parsed = SA_Rfc3339_Time.TryParseRfc3339(dateString, out date);
				if(!parsed) {
					Debug.LogWarning("Date Parsing failed: " + dateString);
				}

				value = (T)System.Convert.ChangeType (date, typeof(T));

			} else {
				value = (T)System.Convert.ChangeType (data, typeof(T));
			}



			return value;
		}


		public string RawData {
			get {
				return _RawData;
			}
		}


		public Dictionary<string, object> Data  {
			get {
				return _Data;
			}
		}

		public bool IsValid {
			get {
				return _IsValid;
			}
		}
	}
}
