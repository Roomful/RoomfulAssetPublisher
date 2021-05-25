////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace SA.Analytics.Google {

	public class GA_RequestCache  {

		private const string DATA_SPLITTER = "|";
		private const string RQUEST_DATA_SPLITTER = "%rps%";

		private const string GA_DATA_CACHE_KEY = "GoogleAnalyticsRequestCache";

		public static void SaveRequest(string cache) {


            GA_CachedRequest r = new GA_CachedRequest(cache, DateTime.Now.Ticks);

			List<GA_CachedRequest> current = CurrenCachedRequests;
			current.Add(r);

			CacheRequests(current);

		}

		public static void SendChashedRequests() {

			List<GA_CachedRequest> current = CurrenCachedRequests;
			foreach(GA_CachedRequest request in current) {
				string HitRequest = request.RequestBody;
				if(GA_Settings.Instance.IsQueueTimeEnabled) {
					HitRequest += "&qt" + request.Delay;
                    GA_Manager.SendSkipCache(HitRequest);
				}

			}

				
			Clear();
		}


		public static void Clear() {
			PlayerPrefs.DeleteKey(GA_DATA_CACHE_KEY);
		}

		public static string SavedData {
			get {
				if(PlayerPrefs.HasKey(GA_DATA_CACHE_KEY)) {
					return PlayerPrefs.GetString(GA_DATA_CACHE_KEY);
				} else {
					return string.Empty;
				}
			}

			set {
				PlayerPrefs.SetString(GA_DATA_CACHE_KEY, value);
			}
		}

		public static void CacheRequests(List<GA_CachedRequest> requests) {

            GA_RequestCacheHolder holder = new GA_RequestCacheHolder();
			

			foreach(GA_CachedRequest request in requests) {
                holder.AddRequestCache(request);
			}

            SavedData = JsonUtility.ToJson(holder);
		}

		public static List<GA_CachedRequest> CurrenCachedRequests {
			get {
				if(SavedData == string.Empty) {
					return new List<GA_CachedRequest>();
				} else {
					try {
                        GA_RequestCacheHolder holder = JsonUtility.FromJson<GA_RequestCacheHolder>(SavedData); 
						return holder.Cache;
					} catch(Exception ex) {
						Clear();
                        Debug.LogError(ex.Message);
						return new List<GA_CachedRequest>();
					}
				}
			}
		}


	}

}
