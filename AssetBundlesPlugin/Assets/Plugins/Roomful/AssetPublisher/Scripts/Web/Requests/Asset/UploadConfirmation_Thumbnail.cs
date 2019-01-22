using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadConfirmation_Thumbnail : BaseWebPackage {

		private const string REQUEST_URL = "/api/v0/asset/upload/thumbnail/link/complete";

		private string _AssetId;

		public UploadConfirmation_Thumbnail (string assetId) {
			_AssetId = assetId;
		}

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);

			return OriginalJSON;
		}
	}
}
