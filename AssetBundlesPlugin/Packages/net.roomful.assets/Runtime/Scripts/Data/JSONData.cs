using System;
using UnityEngine;
using System.Collections.Generic;
using SA.Foundation.Time;
using StansAssets.Foundation;

namespace net.roomful.assets {
	
	public class JSONData  {

		private readonly Dictionary<string, object> _Data = null;
		private readonly string _RawData = string.Empty;

		private readonly bool _IsValid = false;



		public JSONData(string data) {
			try {
				_Data =   Json.Deserialize(data) as Dictionary<string, object>;
				_RawData = data;
				_IsValid = true;

			} catch(Exception ex) {
				Debug.LogError (ex.Message);
				Debug.LogError ("Can't parse JSONData out of: " + data);
			}
		}

		public JSONData(object data) {
			try {
				_Data = (Dictionary<string, object>) data;
				_RawData = Json.Serialize(data);
				_IsValid = true;

			} catch(Exception ex) {
				Debug.LogError (ex.Message);
				Debug.LogError ("Can't parse JSONData out of: " + data);
			}
		}



		public bool HasValue(params string[] keys) { 

			var dict = _Data;
			for(var i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}


			var valueKey = keys[keys.Length - 1];

			if(dict.ContainsKey(valueKey)) {
				return dict[valueKey] != null;
			} else {
				return false;
			}
			
		}

		public T GetValue<T>(params string[] keys) {

			var value = default(T);
			var dict = _Data;
			for(var i = 0; i < keys.Length - 1; i++) {
				dict = (Dictionary<string, object>) dict[keys[i]];
			}


			var valueKey = keys[keys.Length - 1];
			var data = dict[valueKey];

			if(typeof (T) == typeof( DateTime)) {
				
				var dateString = Convert.ToString(data);
				DateTime date;

				var parsed = SA_Rfc3339_Time.TryParseRfc3339(dateString, out date);
				if(!parsed) {
					Debug.LogWarning("Date Parsing failed: " + dateString);
				}

				value = (T)Convert.ChangeType (date, typeof(T));

			} else {
				value = (T)Convert.ChangeType (data, typeof(T));
			}



			return value;
		}


		public string RawData => _RawData;

		public Dictionary<string, object> Data => _Data;

		public bool IsValid => _IsValid;
	}
}
