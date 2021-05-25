////////////////////////////////////////////////////////////////////////////////
//  
// @module Assets Common Lib
// @author Klymentenko Pavel (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

namespace SA.Common.Pattern
{
    public static class SA_Mathf
    {
        public static bool ApproximatelyWithEpsilon(float a, float b, float epsilon) {
            return Mathf.Abs(a - b) < epsilon;
        }
    }
}