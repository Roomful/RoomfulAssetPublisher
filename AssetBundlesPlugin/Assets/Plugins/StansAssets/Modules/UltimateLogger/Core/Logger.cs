////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SA.UltimateLogger {

	public static class Logger  {

		private static ILogHandler _LogHandler = null;

		private static bool AlreadyLogging = false;
		private static List<ILogMessageReceiver> Loggers = new List<ILogMessageReceiver>();


		//--------------------------------------
		// Initialisation
		//--------------------------------------

		static Logger() {
			Application.logMessageReceivedThreaded += LogReceiveHandler;
		}

		public static void Init () {
			Debug.unityLogger.logHandler = LogHandler;



            if (LogHandler is ILogMessageReceiver) {
				ILogMessageReceiver receiver = (LogHandler as ILogMessageReceiver);
				AddLogReceiver (receiver);
			}
		}
			

		//--------------------------------------
		// Public Methods
		//--------------------------------------

		public static string GetSessionLog() {
			return Platfroms.iOS_Bridge.GetSessionLog ();
		}

		public static void ShowSessionLog() {
			Platfroms.iOS_Bridge.ShowSessionLog ();
		} 

		public static void ShowSharingUI() {
			Platfroms.iOS_Bridge.ShowSharingUI ();
		}

		public static void AddLogReceiver(ILogMessageReceiver logger) {
			lock(Loggers) {
				if(!Loggers.Contains(logger))	{
					Loggers.Add(logger);
				}
			}
		}

		public static T GetLogReceiver<T>() where T:class {
			foreach(var logger in Loggers) {
				if(logger is T) {
					return logger as T;
				}
			}
			return null;
		}


		//--------------------------------------
		// Get / Set
		//--------------------------------------

		public static ILogHandler LogHandler {
			get {

				if(_LogHandler ==  null) {

					switch(Application.platform) {
					case RuntimePlatform.IPhonePlayer:

						if(LoggerPlatfromsSettings.Instance.iOS_LogsRecord) {
							Platfroms.iOS_Bridge.Init ();
						}

						if(Settings.iOS_OverrideLogsOutput) {
							_LogHandler =  new  Platfroms.iOS_LogHandler ();
						} else {
							FallbackToDefaultLogger ();
						}

						break;
						
					default:
						FallbackToDefaultLogger ();
						break;
					}
				}

				return _LogHandler;
			}
		}
			


		public static LoggerPlatfromsSettings Settings {
			get {
				return LoggerPlatfromsSettings.Instance;
			}
		}



		//--------------------------------------
		// Private Methods
		//--------------------------------------

		private static void FallbackToDefaultLogger() {
			_LogHandler = Debug.unityLogger.logHandler;
		} 

			
		private static void LogReceiveHandler(string logString, string stackTrace, UnityEngine.LogType logType) {
			//Threads safety lock implementation
			lock(Loggers) {
				//Prevent nasty recursion problems
				if(!AlreadyLogging) {
					try {
						AlreadyLogging = true;
						var logInfo = new UnityLog(logString, stackTrace, logType);

						//Delete any dead loggers and pump them with the new log
						Loggers.RemoveAll(l=>l==null);
						Loggers.ForEach(l=>l.OnLogReceived(logInfo));
					} finally {
						AlreadyLogging = false;
					}
				}
			}
		}


	}


}

