using UnityEngine;
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
				//	Debug.Log (package.GeneratedDataText);
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


//			Debug.Log ("WEB::OUT::" + www.url + " | " + package.GeneratedDataText);

			www.Send ();

			while (www.responseCode == -1) {
				//do nothing while blocking
			}

			if(www.isError) {
				Debug.Log(www.error);
			} else {
				if (www.responseCode == 200) {

					/*string logStrning = CleanUpInput (www.downloadHandler.text);
					Debug.Log ("WEB::IN::" + logStrning);*/

					package.PackageCallbackText (www.downloadHandler.text);
					package.PackageCallbackData(www.downloadHandler.data);
				} else {
					Debug.Log("Response code: "+www.responseCode+", message: "+www.downloadHandler.text);
				}
			}
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