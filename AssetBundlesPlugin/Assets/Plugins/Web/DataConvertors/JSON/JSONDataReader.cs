using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moon.Network.Web
{

    public class JSONDataReader : AttributesBasedDataReader, IDataReader
    {

        public JSONDataReader() {
            IgnoreNullValues = true;
        }

        public override byte[] Data {
            get {
                return System.Text.Encoding.UTF8.GetBytes(Json.Serialize(Body));
            }
        }

    }
}