using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
	public class DownloadIcon : BaseWebPackage {


		private const RequestMethods PackMethodName = RequestMethods.GET;

		public DownloadIcon (string url) : base (url, PackMethodName) {

		}


		public override Dictionary<string, object> GenerateData () {
			var OriginalJSON =  new Dictionary<string, object>();

			return OriginalJSON;
		}

		public override bool IsDataPack => true;
	}
}