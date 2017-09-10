using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

namespace Moon.Network.Web
{

    public class JSONDataWriter :  IDataWriter
    {

        private IRequestCallback m_result;

        public void SetResult(IRequestCallback result) {
            m_result = result;
        }


        public void Parse(byte[] data) {

            string dataAsString =  System.Text.Encoding.UTF8.GetString(data);
            Debug.Log("I have to parse??");

            var obj =  Json.Deserialize(dataAsString);

            Debug.Log(obj);
            if (obj is Dictionary<string, object>) {

                object traget = (object)m_result;
                Parse(obj as Dictionary<string, object>, ref traget);
            }
        }


		public static void Parse(object data, ref object holder) {

			if(data is  Dictionary<string, object> ) {
				Parse ((Dictionary<string, object>) data, ref holder);
			}
		}

        public static void Parse(Dictionary<string, object> data, ref object holder) {
            Debug.Log("I have to parse valid data");

            foreach(var pair in data) {
                
                
				FieldInfo field = GetFiledByName (pair.Key, ref holder);
				if(field != null) {

					Debug.Log(pair.Key);
				//	filed.SetValue(holder,   System.Convert.ChangeType(pair.Value, filed.FieldType));

					Type filedType = field.FieldType;
					field.SetValue(holder,  GetValue(pair.Value, filedType));

				}
			

            }

        }


		//coudl be pverrided should not be static
		public static object GetValue(object filedValue, Type type) {

			object value = GetDefaultValue (type);

			if(type == typeof(DateTime)) {

				string dateString = Convert.ToString(filedValue);
				DateTime date;

				bool parsed = SA.Common.Util.General.TryParseRfc3339(dateString, out date);
				if(!parsed) {
					Debug.LogWarning("Date Parsing failed: " + dateString);
				}

				value = System.Convert.ChangeType (date, type);

			} else {
				value = System.Convert.ChangeType (filedValue, type);
			}

			return value;
		}

		private static object GetDefaultValue(Type t)
		{
			if (t.IsValueType)
				return Activator.CreateInstance(t);

			return null;
		}





		public static FieldInfo GetFiledByName(string filedName, ref object holder) {

			FieldInfo[] fields = holder.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo field in fields) {
				object[] attrs = field.GetCustomAttributes(true);
				foreach (object attr in attrs) {

					if (attr is ParamAttribute) {
						string name = (attr as ParamAttribute).Name;
						if (string.IsNullOrEmpty(name)) {
							name = field.Name;
						}
						if(name.Equals(filedName)) {
							return field;
						}
					}
				}
			}

			return null;
		}

    }
}