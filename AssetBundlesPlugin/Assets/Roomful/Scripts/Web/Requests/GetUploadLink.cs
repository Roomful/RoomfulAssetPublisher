using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class GetUploadLink : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link";

		private string _AssetId;
		private UnityEditor.BuildTarget _Platform;
		private string _FIleName;

		public GetUploadLink (string assetId, UnityEditor.BuildTarget platform, string fileName) : base (PackUrl) {
			_AssetId = assetId;
			_Platform = platform;
			_FIleName = fileName;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("platform", _Platform.ToString());
			OriginalJSON.Add ("fileName", _FIleName);

			return OriginalJSON;
		}

	}
}
