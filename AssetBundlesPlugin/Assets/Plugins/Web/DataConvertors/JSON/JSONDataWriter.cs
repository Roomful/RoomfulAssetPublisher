using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        public static void Parse(Dictionary<string, object> data, ref object holder) {
            Debug.Log("I have to parse valid data");

            foreach(var pair in data) {
                Debug.Log(pair.Key);
                Debug.Log(pair.Value.GetType());
            }
        }

    }
}