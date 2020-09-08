using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class DownloadAsset : BaseWebPackage {
		
		private const RequestMethods PackMethodName = RequestMethods.GET;

		public DownloadAsset (string url) : base (url, PackMethodName) {
			
		}

		public override bool IsDataPack => true;

		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}
	}
}
