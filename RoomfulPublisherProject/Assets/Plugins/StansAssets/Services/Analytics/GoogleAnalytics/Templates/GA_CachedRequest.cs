////////////////////////////////////////////////////////////////////////////////
//  
// @module Google Analytics Plugin
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////



using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


namespace SA.Analytics.Google {

    [Serializable]
	public class GA_CachedRequest  {

		[SerializeField] long  m_timeCreated;
        [SerializeField] string m_requestBody;


		public GA_CachedRequest() {

		}

		public GA_CachedRequest(string body, long ticks)   {
			m_requestBody = body;
			m_timeCreated = ticks;
		}
		
		public long TimeCreated {
			get {
				return m_timeCreated;
			} 

			set {
				m_timeCreated = value;
			}
		}

		public string RequestBody {
			get {
				return m_requestBody;
			}

			set {
				m_requestBody = value;
			}
		}

		public string Delay {
			get {
				System.DateTime CreatedTime = new System.DateTime(TimeCreated);
				double ms = System.DateTime.Now.Subtract(CreatedTime).TotalMilliseconds;

				long LongRep = System.Convert.ToInt64(ms);
				return LongRep.ToString();
			}
		}

		public List<string> DataForJSON {
			get {
				List<string> l = new List<string>();
				l.Add(RequestBody);
				l.Add(TimeCreated.ToString());

				return l;
			}

		
		}
	}
}
