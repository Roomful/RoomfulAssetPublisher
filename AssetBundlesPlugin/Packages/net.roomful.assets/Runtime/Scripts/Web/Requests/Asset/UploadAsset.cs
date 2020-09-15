using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	internal class UploadAsset : BaseWebPackage {

		private const RequestMethods PackMethodName = RequestMethods.PUT;

		public UploadAsset (string packUrl, byte[] data) : base (packUrl, PackMethodName) {
			_PackData = data;
		}

		public override bool IsDataPack => true;

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}