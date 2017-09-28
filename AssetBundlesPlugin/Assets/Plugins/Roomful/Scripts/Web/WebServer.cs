﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RF.AssetWizzard.Network {
	internal class WebServer : SA.Common.Pattern.NonMonoSingleton<WebServer> {
		private static List<Request.BaseWebPackage> DelayedPackages =  new List<Request.BaseWebPackage>();


		public const string HeaderSessionId = "x-session-id";
		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public void Send(Request.BaseWebPackage package) {
			SendRequest (package);
		}

		public void SendDelayedPackages() {
			Debug.Log("SendDelayedPackages" + DelayedPackages.Count);

			foreach(Request.BaseWebPackage p in DelayedPackages) {
				SendRequest(p);
			}


			DelayedPackages.Clear();
		}

		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public static Int32 CurrentTimeStamp {
			get {
				return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			}
		}

		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private void SendRequest(Request.BaseWebPackage package) {
			UnityWebRequest www = null;

			switch (package.MethodName) {
			case RequestMethods.POST:
			case RequestMethods.PUT:
				if (package.IsDataPack) {
					UploadHandlerRaw uploadHandler = new UploadHandlerRaw (package.GeneratedDataBytes);

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

			if (package.Headers.Count > 0) {
				foreach (var pack in package.Headers) {
					www.SetRequestHeader (pack.Key, pack.Value);
				}
			}

			if(AssetBundlesSettings.Instance.ShowWebOutLogs) {
                U.Log(package.Url + ":" + package.MethodName + "::" + www.url + " | " + package.GeneratedDataText, SA.UltimateLogger.DefaultTags.OUT);
            }

            string cleanedUrl = www.url.Replace(" ", "%20");
            www.url = cleanedUrl;

            EditorWebRequest request = new EditorWebRequest(www, package);
            request.Send(() => {

				if (www.isNetworkError || www.isHttpError) {
                    package.RequestFailed(www.responseCode, www.error);
                } else {
                    if (www.responseCode == 200) {
                        string logStrning = CleanUpInput(www.downloadHandler.text);

                        if (AssetBundlesSettings.Instance.ShowWebInLogs) {
                            U.Log(package.Url + "::" + logStrning, SA.UltimateLogger.DefaultTags.IN);
                        }
                        package.PackageCallbackText(www.downloadHandler.text);
                        package.PackageCallbackData(www.downloadHandler.data);
                    } else {
                        package.RequestFailed(www.responseCode, www.downloadHandler.text);

                        if (AssetBundlesSettings.Instance.ShowWebInLogs) {
                            U.Log(package.Url + "::Response code: " + www.responseCode + ", message: " + www.downloadHandler.text, SA.UltimateLogger.DefaultTags.IN);
                        }

                    }
                }
            });
        }

      

        IEnumerator GetText() {
            using (UnityWebRequest request = UnityWebRequest.Get("http://unity3d.com/")) {

                Debug.Log(request.isDone);
                yield return request.Send();
                Debug.Log(request.isDone);

                yield return new WaitForSeconds(2f);

                Debug.Log(request.isDone);
                if (request.isNetworkError) // Error
                {
                    Debug.Log(request.error);
                } else // Success
                  {
                    Debug.Log(request.downloadHandler.text);
                }
            }
        }


        private IEnumerator ExecuteRequest(UnityWebRequest www, Request.BaseWebPackage package) {

   
            yield return www.Send();

           

           

        }

		private static string CleanUpInput(string json) {

			string pattern = "\"assetmesh\":\".*?\"";
			Regex regular = new Regex (pattern);
			return regular.Replace(json, "\"assetmesh\":\"BASE_64_DATA\"");
		}

		public static string ByteToString(byte[] buff) {
			string sbinary = "";

			for (int i = 0; i < buff.Length; i++)
				sbinary += buff[i].ToString("X2"); /* hex format */
			return sbinary.ToLower();
		}
	}
}