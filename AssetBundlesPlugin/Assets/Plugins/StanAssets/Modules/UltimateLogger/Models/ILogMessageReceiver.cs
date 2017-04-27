////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using System.Collections;


namespace SA.UltimateLogger {

	public interface ILogMessageReceiver  {

		void OnLogReceived (UnityLog log);
	}
}
