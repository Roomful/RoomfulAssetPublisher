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
    public interface ILogBridge
    {
        void Init();
        void ShowSharingUI();
        void ShowSessionLog();
        string GetSessionLog();
    }
}
