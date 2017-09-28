////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SA.UltimateLogger.Platfroms {

	public class iOS_LogHandler : LogHandler {

		//--------------------------------------
		// Overrided
		//--------------------------------------

		protected override void LogMessage(string tag, string message) {
			iOS_Bridge.LogMessage (tag, message);
		}





	}
}