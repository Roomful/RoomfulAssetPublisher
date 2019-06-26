using System;
using System.Collections.Generic;
using System.Text;
using SA.Common.Models;

namespace RF.AssetWizzard.Network.Request {
	
	public abstract class BaseWebPackage  
	{
		
		public Action<string> PackageCallbackText = delegate {};
		public Action<byte[]> PackageCallbackData = delegate {};
        public Action<long> PackageCallbackError = delegate {};
        public Action<float> DownloadProgress= delegate {};
        public Action<float> UploadProgress = delegate {};

        protected Dictionary<string, string> m_headers = new Dictionary<string, string>();
		protected byte[] m_packData;
        private Error m_error;

        public BaseWebPackage() {
            m_headers.Add(WebServer.HeaderSessionId, AssetBundlesSettings.Instance.SessionId);
            m_headers.Add(WebServer.HeaderPublisherVersion, AssetBundlesSettings.Instance.PublisherCurrentVersion);
        }

		public abstract string Url { get ; }

        public virtual RequestMethods MethodName { 
            get { 
                return RequestMethods.POST;    
            } 
        }

		public virtual Int32 Timeout {
			get {
				return 10;
			}
		}

		public virtual bool IsDataPack {
			get {
				return false;
			}
		}

		public Dictionary<string, string> Headers {
			get {
				return m_headers;
			}
		}

		public byte[] GeneratedDataBytes {
			get {
				return m_packData;
			}
		}

		public string GeneratedDataText {
			get {
				return SA.Common.Data.Json.Serialize (GetRequestData ());
			}
		}

        public Error Error {
            get {
                return m_error;
            }
        }

		public virtual void Send() {
			StringBuilder s = new StringBuilder();
			foreach (var header in m_headers) {
				s.Append(header.Key + " " + header.Value + ";");
			}
			WebServer.Instance.Send(this);
		}


        public virtual void RequestFailed(long code,  string message) {
            m_error = new Error((int)code, message);
            PackageCallbackError(code); 
        }


        public virtual Dictionary<string, object> GetRequestData() {
            return new Dictionary<string, object>();
        }
    }
}