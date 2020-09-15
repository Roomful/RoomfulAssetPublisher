﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using StansAssets.Foundation;
using StansAssets.Foundation.Patterns;
using UnityEngine.Networking;


namespace net.roomful.assets.Network {
	public class WebServer : Singleton<WebServer> {
		private static readonly List<Request.BaseWebPackage> DelayedPackages =  new List<Request.BaseWebPackage>();

        public static Action<Request.BaseWebPackage> OnRequestFailed = delegate { };

		public const string HeaderSessionId = "x-session-id";
		public const string HeaderPublisherVersion = "X-Asset-Version";
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void Send(Request.BaseWebPackage package) {
			SendRequest (package);
		}

		public void SendDelayedPackages() {
			Debug.Log("SendDelayedPackages" + DelayedPackages.Count);

			foreach(var p in DelayedPackages) {
				SendRequest(p);
			}


			DelayedPackages.Clear();
		}

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public static Int32 CurrentTimeStamp => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private void SendRequest(Request.BaseWebPackage package) {
			UnityWebRequest www = null;
			ServicePointManager.ServerCertificateValidationCallback += Validator;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            switch (package.MethodName) {
			case RequestMethods.POST:
			case RequestMethods.PUT:
				if (package.IsDataPack) {
					var uploadHandler = new UploadHandlerRaw (package.GeneratedDataBytes);

					www = UnityWebRequest.Put (package.Url, package.GeneratedDataBytes);
					www.uploadHandler = uploadHandler;

				} else {
					www = UnityWebRequest.Put(AssetBundlesSettings.WEB_SERVER_URL + package.Url, package.GeneratedDataText);
				}
					break;
			case RequestMethods.GET: 

				if (package.IsDataPack) {
					www = UnityWebRequest.Get (package.Url);
				} else {
					www = UnityWebRequest.Get (AssetBundlesSettings.WEB_SERVER_URL + package.Url);
				}

					break;
			default:
				Debug.LogWarning ("Incorrect request name!");
				break;
			}

			www.method = package.MethodName.ToString();
#if UNITY_2018_1_OR_NEWER
            www.certificateHandler = new AlwaysTrueCertificateHandler();
#endif
            if (package.Headers.Count > 0) {
				foreach (var pack in package.Headers) {
					www.SetRequestHeader (pack.Key, pack.Value);
				}
			}
#if UNITY_EDITOR
			if(AssetBundlesSettings.Instance.m_showWebOutLogs) {

                var h = Json.Serialize(package.Headers);
                Debug.Log(AssetBundlesSettings.WEB_SERVER_URL + package.Url + ":" + package.MethodName + "::" + www.url + " | " + package.GeneratedDataText + " | headers: " + h);

            }
#endif
            var cleanedUrl = www.url.Replace(" ", "%20");
            www.url = cleanedUrl;

            var request = new EditorWebRequest(www, package);
            request.Send(() => {
	            
				if (www.isNetworkError || www.isHttpError) {
                    package.RequestFailed(www.responseCode, www.error);
                    OnRequestFailed(package);
                } else {
                    if (www.responseCode == 200) {

#if UNITY_EDITOR
                        if (AssetBundlesSettings.Instance.m_showWebInLogs) {

                            var logStrning = CleanUpInput(www.downloadHandler.text);
                            Debug.Log(AssetBundlesSettings.WEB_SERVER_URL + package.Url + "::" + logStrning);

                        }
#endif
	                    package.PackageCallbackText(www.downloadHandler.text);
                        package.PackageCallbackData(www.downloadHandler.data);
                    } else {

                        package.RequestFailed(www.responseCode, www.downloadHandler.text);
                        if (AssetBundlesSettings.Instance.m_showWebInLogs) {
#if UNITY_EDITOR
	                        Debug.Log(AssetBundlesSettings.WEB_SERVER_URL + package.Url + "::Response code: " + www.responseCode + ", message: " + www.downloadHandler.text);
#endif
                        }

                        OnRequestFailed(package);

                    }
                }
            });
        }

		private bool Validator(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors) {
			return true;
		}
		

		private static string CleanUpInput(string json) {

			var pattern = "\"assetmesh\":\".*?\"";
			var regular = new Regex (pattern);
			return regular.Replace(json, "\"assetmesh\":\"BASE_64_DATA\"");
		}

		public static string ByteToString(byte[] buff) {
			var sbinary = "";

			for (var i = 0; i < buff.Length; i++)
				sbinary += buff[i].ToString("X2"); /* hex format */
			return sbinary.ToLower();
		}
#if UNITY_2018_1_OR_NEWER
        private class AlwaysTrueCertificateHandler : CertificateHandler {
            protected override bool ValidateCertificate(byte[] certificateData) {
                return true;
            }
        }
#endif
    }
}