using System.Collections.Generic;
using StansAssets.Foundation;
using StansAssets.Foundation.Models;

namespace net.roomful.assets
{
    public class WebPackageResult
    {
        public byte[] Data;
        public readonly string DataAsText;

        Dictionary<string, object> m_DataAsDictionary;

        public Dictionary<string, object> DataAsDictionary
        {
            get
            {
                if (m_DataAsDictionary != null)
                    return m_DataAsDictionary;
                
                m_DataAsDictionary =  Json.Deserialize(DataAsText) as Dictionary<string, object>;
                return m_DataAsDictionary;
            }
        }
        
        Error m_Error;

        public bool HasError => m_Error != null;
        
        public bool IsSucceeded => !HasError;
        public bool IsFailed => HasError;
        

        public WebPackageResult(string dataAsText, byte[] data)
        {
            DataAsText = dataAsText;
            Data = data;
        }

        public WebPackageResult(Error error)
        {
            m_Error = error;
        }
    }
}
