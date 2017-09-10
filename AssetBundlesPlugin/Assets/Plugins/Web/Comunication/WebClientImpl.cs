using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using System.Collections.Generic;

using UnityEngine;


namespace Moon.Network.Web
{

    public class WebClientImpl : IWebRequest 
    {

        private string m_url;
        private WebClient m_client;
        private Dictionary<string, string> m_requestParams = new Dictionary<string, string>();
        private byte[] m_requestData = null;
        private Action<WebResponce> m_callback;



        private static bool s_inited = false;


        public WebClientImpl() {
            m_client = new WebClient();
        }


        public void AddHeader(string key, string value = null) {
            if(string.IsNullOrEmpty(value)) {
                m_client.Headers.Add(key);
            } else {
                m_client.Headers.Add(key, value);
            }
        }

        public void SetData(byte[] data) {
            m_requestData = data;
        }

        public void AddParam(string key, string value) {
            m_requestParams[key] = value;
        }

        public void SetURL(string url) {
            m_url = url;
        }



        public void Send(Method methods, Action<WebResponce> callback) {

            Init();

            m_callback = callback;

            Uri uri = null;
            if (methods == Method.GET) {

                uri = new Uri(m_url + ParamsString());
                m_client.DownloadDataCompleted += OnDownloadDataCompleted;
                m_client.DownloadDataAsync(uri);

            } else {
                uri = new Uri(m_url);
                m_client.UploadDataCompleted += OnUploadDataCompleted;
                m_client.UploadDataAsync(uri, methods.ToString(), m_requestData);
            }


            Debug.Log(methods + ": " + uri.OriginalString + " | " + System.Text.Encoding.UTF8.GetString(m_requestData));
        }


        private string ParamsString() {

            string paramsString = string.Empty;
            bool first = true;

            foreach (var pair in m_requestParams) {
                if(first) {
                    paramsString += "?";
                    first = false;
                } else {
                    paramsString += "&";
                }
                paramsString += pair.Key + "=" + pair.Value;
            }

            return paramsString;
        }



        private void OnUploadDataCompleted(object sender, UploadDataCompletedEventArgs e) {
            Finish(e.Result, e.Error);
        }

        private void OnDownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {

            if (e.Error != null) {
                Finish(null, e.Error);
            } else {
                Finish(e.Result, null);
            }

        }

        private void Finish(byte[] result, Exception e) {
            WebResponce resp = new WebResponce(result, e);
            m_callback(resp);
        }


        public static void Init() {
            if(s_inited) {
                return;
            }

            s_inited = true;
            ServicePointManager.ServerCertificateValidationCallback = OnRemoteCertificateValidation;
         //   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            //  ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
           // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }


        public static bool OnRemoteCertificateValidation(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {

            bool isOk = true;
            // If there are errors in the certificate chain,
            // look at each error to determine the cause.
            if (sslPolicyErrors != SslPolicyErrors.None) {
                for (int i = 0; i < chain.ChainStatus.Length; i++) {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown) {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid) {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }



    }
}