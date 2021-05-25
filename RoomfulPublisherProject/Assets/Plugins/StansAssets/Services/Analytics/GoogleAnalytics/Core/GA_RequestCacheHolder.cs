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


    [Serializable]
	public class GA_RequestCacheHolder  {
        [SerializeField] List<GA_CachedRequest> m_cache;


        public void AddRequestCache(GA_CachedRequest request) {
            Cache.Add(request);
        }


        public List<GA_CachedRequest> Cache {
            get {
                if (m_cache == null) {
                    m_cache = new List<GA_CachedRequest>();
                }

                return m_cache;
            }
        }
    }

}
