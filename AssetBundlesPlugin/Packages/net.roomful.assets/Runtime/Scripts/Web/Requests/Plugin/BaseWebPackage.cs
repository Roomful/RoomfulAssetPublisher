using System;
using System.Collections.Generic;
using System.Text;
using Models;
using StansAssets.Foundation;

namespace net.roomful.assets.Network.Request
{
    public abstract class BaseWebPackage
    {
        protected string _Url;
        private readonly int _TimeStamp;
        private readonly RequestMethods _MethodName = RequestMethods.POST;
        protected byte[] _PackData;

        public Action<string> PackageCallbackText = delegate { };
        public Action<byte[]> PackageCallbackData = delegate { };
        public Action<long> PackageCallbackError = delegate { };

        public Action<float> DownloadProgress = delegate { };
        public Action<float> UploadProgress = delegate { };

        protected Dictionary<string, string> _Headers = new Dictionary<string, string>();

        private Error m_error = null;

        //--------------------------------------
        //  Initialization
        //--------------------------------------

        public BaseWebPackage(string url)
        {
            _Url = url;
            _TimeStamp = WebServer.CurrentTimeStamp;

            _Headers.Add(WebServer.HeaderSessionId, AssetBundlesSettings.Instance.SessionId);
            _Headers.Add(WebServer.HeaderPublisherVersion, AssetBundlesSettings.Instance.PublisherCurrentVersion);
        }

        public BaseWebPackage(string url, RequestMethods methodName) : this(url)
        {
            _MethodName = methodName;
        }

        public BaseWebPackage(string url, RequestMethods methodName, byte[] bodyData) : this(url, methodName)
        {
            _PackData = bodyData;
        }

        //--------------------------------------
        // Get / SET
        //--------------------------------------


        public string Url => _Url;

        public RequestMethods MethodName => _MethodName;

        public Int32 TimeStamp => _TimeStamp;

        public virtual Int32 Timeout => 10;

        public virtual bool AuthenticationRequired => true;

        public virtual bool IsDataPack => false;

        public Dictionary<string, string> Headers => _Headers;

        public byte[] GeneratedDataBytes => _PackData;

        public string GeneratedDataText => Json.Serialize(GenerateData());

        public Error Error => m_error;

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        protected void AddToUrl(string newPart)
        {
            _Url += newPart;
        }

        public virtual void Send()
        {
            var s = new StringBuilder();
            foreach (var header in _Headers)
            {
                s.Append(header.Key + " " + header.Value + ";");
            }

            WebServer.Instance.Send(this);
        }


        public virtual void RequestFailed(long code, string message)
        {
            m_error = new Error((int) code, message);
            PackageCallbackError(code);
        }


        public abstract Dictionary<string, object> GenerateData();
    }
}