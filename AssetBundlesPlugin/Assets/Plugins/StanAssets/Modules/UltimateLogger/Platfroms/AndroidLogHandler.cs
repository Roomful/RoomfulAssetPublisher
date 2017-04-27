////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Alexey Yaremenko (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.UltimateLogger.Platforms
{
    public class AndroidLogHandler : LogHandler
    {
        protected override void LogMessage(string logtype, string message)
        {
			SA.UltimateLogger.Utils.UnityMainThreadDispatcher.Instance.Enqueue (() =>
				{
					AndroidBridge.LogMessage(logtype, message);
				});
        }
    }
}
