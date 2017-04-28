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
	public sealed class EditorStub : ILogBridge
	{
		public void Init() {}

		public void ShowSharingUI() {}

		public void ShowSessionLog() {}

		public string GetSessionLog() { return string.Empty; }
	}
}
