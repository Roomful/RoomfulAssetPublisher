﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadConfirmation : BaseWebPackage {

		private const string PackUrl = "/api/v0/asset/upload/link/complete";

		private string _AssetId;
		private UnityEditor.BuildTarget _Platform;

		public UploadConfirmation (string assetId, UnityEditor.BuildTarget platform) : base (PackUrl) {
			_AssetId = assetId;
			_Platform = platform;
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			OriginalJSON.Add ("asset", _AssetId);
			OriginalJSON.Add ("platform", _Platform.ToString());

			return OriginalJSON;
		}
	}
}
