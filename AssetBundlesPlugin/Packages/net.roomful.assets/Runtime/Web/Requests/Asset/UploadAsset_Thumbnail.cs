using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class UploadAsset_Thumbnail : BaseWebPackage {

		private const RequestMethods PackMethodName = RequestMethods.PUT;

		public UploadAsset_Thumbnail (string packUrl, Texture2D icon) : base (packUrl, PackMethodName) {
			_PackData = icon.EncodeToPNG ();
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