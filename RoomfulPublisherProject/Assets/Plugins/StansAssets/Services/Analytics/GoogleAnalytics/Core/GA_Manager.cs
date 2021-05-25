////////////////////////////////////////////////////////////////////////////////
//
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets)
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////




using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;


#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using UnityEngine.SceneManagement;
#endif


namespace SA.Analytics.Google {


	public class GA_Manager : MonoBehaviour {


		public const string GOOGLE_ANALYTICS_CLIENTID_PREF_KEY = "google_analytics_clientid_pref_key";
		public const string GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED = "system_info_submited";
		public const string SYSTEM_INFO = "SystemInfo";


		private static GA_Manager Instance = null;

		private static string _ClientId;

		private static GA_Client cleint = null;
		private static string CurrentLevelName;


		private static bool IsSessionStarted = false;



		//--------------------------------------
		// INITIALIZE
		//--------------------------------------


		void Awake() {

			if (Instance != null) {
				DestroyImmediate(gameObject);
				return;
			} else  {
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}

			name = "Google Analytics";



			GenerateClientId();
			cleint =  new GA_Client(_ClientId);


			if(!IsSessionStarted) {

				Client.CreateHit(GA_HitType.SCREENVIEW);
				Client.StartSession();
				Client.SetUserLanguage(Application.systemLanguage.ToString());
				Client.SetScreenResolution(Screen.width, Screen.height);
				Client.Send();

				IsSessionStarted = true;
			}

			SendFirstScreenHit();
			SubmitSystemInfo();



			if(GA_Settings.Instance.AutoExceptionTracking) {
				Application.logMessageReceived += HandleLog;
			}


			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
			#else

			SceneManager.sceneLoaded += (Scene arg0, LoadSceneMode arg1) => {
				TrackNewLevel();
			};

			#endif



		}


		public static void SetTrackingId(string TrackingId) {
			StartTracking();
			cleint.GenerateHeaders(TrackingId);
		}


		public static void StartTracking() {
			if(Instance == null) {
				GameObject an = new GameObject("Google Analytics");
				an.AddComponent<GA_Manager>();
			}
		}

		//--------------------------------------
		// EVENTS
		//--------------------------------------


		#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
		void OnLevelWasLoaded(int level) {
			TrackNewLevel();
		}
		#else



		#endif




		void OnApplicationPause(bool paused) {

			if(!GA_Settings.Instance.AutoAppResumeTracking) {
				return;
			}

			if (paused) {
				Client.CreateHit(GA_HitType.APPVIEW);
				//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Enter Background");

				Client.EndSession();
				Client.Send();
			} else {
				Client.CreateHit(GA_HitType.APPVIEW);
				//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Enter Foreground");

				Client.StartSession();
				Client.Send();
			}
		}


		void OnApplicationQuit() {
			if(!GA_Settings.Instance.AutoAppQuitTracking) {
				return;
			}

			Client.CreateHit(GA_HitType.APPVIEW);
			//Client.SetScreenName(GoogleAnalyticsSettings.Instance.AppName + " - Quit");
			Client.EndSession();
			Client.Send();
		}



		void HandleLog (string logString , string stackTrace, LogType type) {

			if(!GA_Settings.Instance.AutoExceptionTracking) {
				return;
			}

            if (!Application.isPlaying) {
                return;
            }


            if (type == LogType.Exception) {
				Client.CreateHit (GA_HitType.EXCEPTION);
				Client.SetExceptionDescription (logString);
				Client.SetScreenName (LoadedLevelName);
				Client.SetDocumentTitle (stackTrace);
				Client.SetIsFatalException (false);
				Client.Send ();
			}


			if(type == LogType.Error) {

                if(logString.Length >= 150) {
                    logString = logString.Substring(0, 149);
                }

                if (stackTrace.Length >= 150) {
                    stackTrace = stackTrace.Substring(0, 149);
                }

				Client.CreateHit (GA_HitType.EXCEPTION);
				Client.SetExceptionDescription (logString);
				Client.SetScreenName (LoadedLevelName);
				Client.SetDocumentTitle (stackTrace);
				Client.SetIsFatalException (false);
				Client.Send ();
			}

		}


		//--------------------------------------
		//  GET / SET
		//--------------------------------------


		public static GA_Client Client {
			get {
				if(cleint == null) {
					StartTracking();
				}

				return cleint;
			}
		}

		public static string ClientId {
			get {
				return _ClientId;
			}
		}

		public static string LoadedLevelName {
			get {
				#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				return  Application.loadedLevelName;
				#else
				return SceneManager.GetActiveScene().name;
				#endif
			}
		}


		//--------------------------------------
		//  PUBLIC METHODS
		//--------------------------------------

		public static void SubmitSystemInfo() {
			if(GA_Settings.Instance.SubmitSystemInfoOnFirstLaunch) {
				if(!PlayerPrefs.HasKey(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED)) {
					PlayerPrefs.SetInt(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED, 1);


					Client.SendEventHit(SYSTEM_INFO, "deviceModel", SystemInfo.deviceModel);
					Client.SendEventHit(SYSTEM_INFO, "deviceName", SystemInfo.deviceName);
					Client.SendEventHit(SYSTEM_INFO, "deviceType", SystemInfo.deviceType.ToString());
					Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceID", SystemInfo.graphicsDeviceID.ToString(), SystemInfo.graphicsDeviceID);
					Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVendorID", SystemInfo.graphicsDeviceVendorID.ToString(), SystemInfo.graphicsDeviceVendorID);
					Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceName", SystemInfo.graphicsDeviceName);
					Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
					Client.SendEventHit(SYSTEM_INFO, "graphicsDeviceVersion", SystemInfo.graphicsDeviceVersion);
					Client.SendEventHit(SYSTEM_INFO, "graphicsShaderLevel", SystemInfo.graphicsShaderLevel.ToString(), SystemInfo.graphicsShaderLevel);



					Client.SendEventHit(SYSTEM_INFO, "graphicsMemorySize", SystemInfo.graphicsMemorySize.ToString() + "MB", SystemInfo.graphicsMemorySize);
					Client.SendEventHit(SYSTEM_INFO, "systemMemorySize", SystemInfo.systemMemorySize.ToString() + "MB", SystemInfo.systemMemorySize);
					Client.SendEventHit(SYSTEM_INFO, "systemLanguage", Application.systemLanguage.ToString());


					Client.SendEventHit(SYSTEM_INFO, "operatingSystem", SystemInfo.operatingSystem);
					Client.SendEventHit(SYSTEM_INFO, "processorType", SystemInfo.processorType);
					Client.SendEventHit(SYSTEM_INFO, "processorCount", SystemInfo.processorCount.ToString(), SystemInfo.processorCount);




					Client.SendEventHit(SYSTEM_INFO, "supportsAccelerometer", SystemInfo.supportsAccelerometer ? "true" : "false", SystemInfo.supportsAccelerometer ? 1 : 0);
					Client.SendEventHit(SYSTEM_INFO, "supportsLocationService", SystemInfo.supportsLocationService ? "true" : "false", SystemInfo.supportsLocationService ? 1 : 0);
					Client.SendEventHit(SYSTEM_INFO, "supportsVibration", SystemInfo.supportsVibration ? "true" : "false", SystemInfo.supportsVibration ? 1 : 0);


					Client.SendEventHit(SYSTEM_INFO, "supportsShadows", SystemInfo.supportsShadows ? "true" : "false", SystemInfo.supportsShadows ? 1 : 0);


				}
			}
		}


		public static void RestorePrefKeys() {
			PlayerPrefs.SetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY, _ClientId);
			PlayerPrefs.SetInt(GOOGLE_ANALYTICS_SYSTEM_INFO_SUBMITED, 1);
		}


		public static void Send(string request) {

            if(Instance == null) {
                return;
            }

			#if UNITY_WEBPLAYER
			#if !UNITY_EDITOR
			Application.ExternalCall("SendGA", Client.AnalyticsHost, request);
			#endif
			#else
			byte[] data = System.Text.Encoding.UTF8.GetBytes(request);
			if(GA_Settings.Instance.IsRequetsCachingEnabled) {
				Instance.StartCoroutine(Instance.SendAnalytics(data, request));
			} else {
				SendSkipCache(request);
			}

			#endif
		}

		public static void SendSkipCache(string request) {
			byte[] data = System.Text.Encoding.UTF8.GetBytes(request);
			Client.GenerateWWW(data);
		}



		private IEnumerator SendAnalytics (byte[] data, string cache) {
            // Start a download of the given URL
            UnityWebRequest request = Client.GenerateWWW(data);

            // Wait for download to complete
            yield return request.SendWebRequest();


            if (request.error != null) {
                GA_RequestCache.SaveRequest(cache);
			} else {
                GA_RequestCache.SendChashedRequests();
			}



		}


		//--------------------------------------
		//  PRIVATE METHODS
		//--------------------------------------

		private static void SendFirstScreenHit() {
			if(GA_Settings.Instance.AutoLevelTracking) {

				CurrentLevelName = LoadedLevelName;

				Client.CreateHit(GA_HitType.APPVIEW);
				Client.SetScreenResolution(Screen.currentResolution.width, Screen.currentResolution.height);
				Client.SetViewportSize(Screen.width, Screen.height);
				Client.SetUserLanguage(Application.systemLanguage.ToString());
				Client.SetScreenName(GA_Settings.Instance.LevelPrefix + CurrentLevelName + GA_Settings.Instance.LevelPostfix);

				Client.Send();
			}
		}

		private static void TrackNewLevel() {
			if(GA_Settings.Instance.AutoLevelTracking) {

				if(!CurrentLevelName.Equals(LoadedLevelName)) {
					CurrentLevelName = LoadedLevelName;
					Client.SendScreenHit(GA_Settings.Instance.LevelPrefix + CurrentLevelName + GA_Settings.Instance.LevelPostfix);
				}
			}
		}



		private static void GenerateClientId() {
			if(PlayerPrefs.HasKey(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY)) {
				_ClientId = PlayerPrefs.GetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY);
			} else {
				#if UNITY_ANDROID || UNITY_BLACKBERRY
				_ClientId = RandomString(32);
				#else
				_ClientId = RandomString(8) + SystemInfo.deviceUniqueIdentifier;
				#endif

				PlayerPrefs.SetString(GOOGLE_ANALYTICS_CLIENTID_PREF_KEY, _ClientId);
			}
		}


		private static System.Random random = new System.Random((int)System.DateTime.Now.Ticks);
		private static string RandomString(int size) {

			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			char ch;
			for (int i = 0; i < size; i++) {
				ch = System.Convert.ToChar(System.Convert.ToInt32(System.Math.Floor(26 * random.NextDouble() + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}


	}

}
