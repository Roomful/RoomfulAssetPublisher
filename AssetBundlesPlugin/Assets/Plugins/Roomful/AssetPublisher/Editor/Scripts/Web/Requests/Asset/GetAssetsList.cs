using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SA.Common.Data;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public abstract class GetAssetsList : BaseWebPackage {
        private List<ReleaseStatus> m_releaseStatuses = new List<ReleaseStatus>();
		private int m_offset = 0;
		private int m_size = 10;

        public GetAssetsList(int offset, int size) {
            m_offset = offset;
            m_size = size;
        }

        public GetAssetsList(int offset, int size, List<ReleaseStatus> releaseStatuses): this(offset, size) {
			m_releaseStatuses.AddRange (releaseStatuses);
		}


		public override Dictionary<string, object> GetRequestData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();
            OriginalJSON.Add("releaseStatuses", m_releaseStatuses.Select(s => s.ToString()).ToArray());
            OriginalJSON.Add ("offset", m_offset);
			OriginalJSON.Add ("size", m_size);
			Debug.Log(Json.Serialize(OriginalJSON));
			return OriginalJSON;
		}
	}
}
