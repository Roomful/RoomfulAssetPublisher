using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SA.Common.Models;

namespace RF.AssetWizzard.Network.Request {
	public abstract class BaseWebPackage  {
		
		protected string _Url;
		private int _TimeStamp;
		private RequestMethods _MethodName = RequestMethods.POST;
		protected byte[] _PackData;

		public Action<string> PackageCallbackText = delegate {};
		public Action<byte[]> PackageCallbackData = delegate {};
        public Action<long> PackageCallbackError = delegate { };

        public Action<float> DownloadProgress= delegate { };
        public Action<float> UploadProgress = delegate { };

        protected Dictionary<string, string> _Headers = new Dictionary<string, string>();

        private Error m_error = null;

		//--------------------------------------
		//  Initialization
		//--------------------------------------

		public BaseWebPackage(string url) {
			_Url = url;
			_TimeStamp = WebServer.CurrentTimeStamp;
            
			_Headers.Add(WebServer.HeaderSessionId, AssetBundlesSettings.Instance.SessionId);
			_Headers.Add(WebServer.HeaderPublisherVersion, AssetBundlesSettings.Instance.PublisherCurrentVersion);
		}

		public BaseWebPackage(string url, RequestMethods methodName) : this(url) {
			_MethodName = methodName;
		}

		public BaseWebPackage(string url, RequestMethods methodName, byte[] bodyData) : this(url, methodName) {
			_PackData = bodyData;
		}

		//--------------------------------------
		// Get / SET
		//--------------------------------------


		public string Url {
			get {
				return _Url;
			}
		}

		public RequestMethods MethodName {
			get {
				return _MethodName;
			}
		}

		public Int32 TimeStamp {
			get {
				return _TimeStamp;
			}
		}

		public virtual Int32 Timeout {
			get {
				return 10;
			}
		}

		public virtual bool AuthenticationRequired {
			get {
				return true;
			}
		}

		public virtual bool IsDataPack {
			get {
				return false;
			}
		}

		public Dictionary<string, string> Headers {
			get {
				return _Headers;
			}
		}

		public byte[] GeneratedDataBytes {
			get {
				return _PackData;
			}
		}

		public string GeneratedDataText {
			get {
				return SA.Common.Data.Json.Serialize (GenerateData ());
			}
		}

        public Error Error {
            get {
                return m_error;
            }
        }

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		protected void AddToUrl(string newPart) {
			_Url += newPart;
		}

		public virtual void Send() {
			StringBuilder s = new StringBuilder();
			foreach (var header in _Headers) {
				s.Append(header.Key + " " + header.Value + ";");
			}
			WebServer.Instance.Send(this);
		}


        public virtual void RequestFailed(long code,  string message) {
            m_error = new Error((int)code, message);
            PackageCallbackError(code); 
        }


        public abstract Dictionary<string, object> GenerateData();
	}
}