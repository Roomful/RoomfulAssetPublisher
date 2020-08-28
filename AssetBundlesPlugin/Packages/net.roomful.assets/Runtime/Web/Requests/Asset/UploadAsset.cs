using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UploadAsset : BaseWebPackage {

		private const RequestMethods PackMethodName = RequestMethods.PUT;

		public UploadAsset (string packUrl, byte[] data) : base (packUrl, PackMethodName) {
			_PackData = data;
		}

		public override bool IsDataPack {
			get {
				return true;
			}
		}

		public override Dictionary<string, object> GenerateData () {
			Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}