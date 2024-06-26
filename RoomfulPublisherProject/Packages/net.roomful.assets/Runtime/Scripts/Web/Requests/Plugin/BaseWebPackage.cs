using System;
using System.Collections.Generic;
using System.Text;
using StansAssets.Foundation;
using StansAssets.Foundation.Models;

namespace net.roomful.assets
{
    public abstract class BaseWebPackage
    {
        protected string m_Url;
        readonly int m_TimeStamp;
        readonly RequestMethods m_MethodName = RequestMethods.POST;
        protected byte[] m_PackData;

        public Action<string> PackageCallbackText = delegate { };
        public Action<byte[]> PackageCallbackData = delegate { };
        public Action<long> PackageCallbackError = delegate { };

        public Action<float> DownloadProgress = delegate { };
        public Action<float> UploadProgress = delegate { };

        public Action<WebPackageResult> Callback = delegate { };

        protected readonly Dictionary<string, string> m_Headers = new Dictionary<string, string>();

        Error m_Error = null;
        public virtual bool ShouldDisplayAnErrorPopup => true;

        //--------------------------------------
        //  Initialization
        //--------------------------------------

        public BaseWebPackage(string url)
        {
            m_Url = url;
            m_TimeStamp = WebServer.CurrentTimeStamp;

            m_Headers.Add(WebServer.HeaderSessionId, AssetBundlesSettings.Instance.SessionId);
            m_Headers.Add(WebServer.HeaderPublisherVersion, AssetBundlesSettings.Instance.PublisherCurrentVersion);
        }

        public BaseWebPackage(string url, RequestMethods methodName) : this(url)
        {
            m_MethodName = methodName;
        }

        public BaseWebPackage(string url, RequestMethods methodName, byte[] bodyData) : this(url, methodName)
        {
            m_PackData = bodyData;
        }

        //--------------------------------------
        // Get / SET
        //--------------------------------------


        public string Url => m_Url;

        public RequestMethods MethodName => m_MethodName;

        public Int32 TimeStamp => m_TimeStamp;

        public virtual Int32 Timeout => 10;

        public virtual bool AuthenticationRequired => true;

        public virtual bool IsDataPack => false;

        public Dictionary<string, string> Headers => m_Headers;

        public byte[] GeneratedDataBytes => m_PackData;

        public string GeneratedDataText => Json.Serialize(GenerateData());

        public Error Error => m_Error;

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        protected void AddToUrl(string newPart)
        {
            m_Url += newPart;
        }

        
        public virtual void Send(Action<WebPackageResult> callback = null)
        {
            var s = new StringBuilder();
            foreach (var header in m_Headers)
            {
                s.Append(header.Key + " " + header.Value + ";");
            }

            if (callback != null)
            {
                this.Callback += callback;
            }

            WebServer.Instance.Send(this);
        }


        public virtual void RequestFailed(long code, string message)
        {
            m_Error = new Error((int) code, message);
            PackageCallbackError(code);
            
            Callback.Invoke(new WebPackageResult(m_Error));
        }


        public abstract Dictionary<string, object> GenerateData();
    }
}
