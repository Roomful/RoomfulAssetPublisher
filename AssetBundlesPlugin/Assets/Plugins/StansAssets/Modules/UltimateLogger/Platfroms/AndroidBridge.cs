////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.UltimateLogger.Platforms
{
    public sealed class AndroidBridge : ILogBridge
    {
        private const string CLASS_NAME = "com.stansassets.logger.core.Bridge";

        public void Init() {
            AndroidProxyPool.CallStatic(CLASS_NAME, "Init");
        }

        public void ShowSharingUI() {
            AndroidProxyPool.CallStatic(CLASS_NAME, "ShowSharingUI");
        }

        public void ShowSessionLog() {
            AndroidProxyPool.CallStatic(CLASS_NAME, "ShowSessionLog");
        }

        public string GetSessionLog() {
#if UNITY_ANDROID
            return AndroidProxyPool.CallStatic<string>(CLASS_NAME, "GetSessionLog");
#else
            return string.Empty;
#endif
        }

		public static void LogMessage(string tag, string message)
        {
			AndroidProxyPool.CallStatic(CLASS_NAME, "LogMessage", tag, message);
        }
    }
}
