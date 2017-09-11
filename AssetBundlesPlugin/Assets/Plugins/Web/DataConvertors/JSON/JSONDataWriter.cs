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

            var obj =  Json.Deserialize(dataAsString);
            if (obj is Dictionary<string, object>) {
                object traget = (object)m_result;
                Parse(obj as Dictionary<string, object>, ref traget);
            }
        }


		public void Parse(object data, ref object holder) {

			if(data is  Dictionary<string, object> ) {
				Parse ((Dictionary<string, object>) data, ref holder);
			}
		}

        public void Parse(Dictionary<string, object> data, ref object holder) {
  
            foreach(var pair in data) {
 
				FieldInfo field = GetFiledByName (pair.Key, ref holder);
				if(field != null) {
					Type filedType = field.FieldType;
					field.SetValue(holder,  GetValue(pair.Value, filedType));

				}
            }

        }

		public virtual object GetValue(object filedValue, Type type) {


			return  System.Convert.ChangeType(filedValue, type); 
		}



		public FieldInfo GetFiledByName(string filedName, ref object holder) {

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


        protected object GetDefaultValue(Type t) {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }


    }
}